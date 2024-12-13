using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Responses;
using HippAdministrata.Models.Requests;
using HippAdministrata.Services.Interface;
using HippAdministrata.Repositories.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Data;

namespace HippAdministrata.Services.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;
        private readonly ApplicationDbContext _dbContext;
        public JwtService(IUserRepository userRepository, IConfiguration configuration, ILogger<JwtService> logger, ApplicationDbContext dbContext)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
            _dbContext = dbContext;
        }

        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "Unknown")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(request.Name);

                if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                    return null;


                var token = GenerateJwtToken(user);


                return new LoginResponse { Token = token, Role = user.Role?.RoleName };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during authentication.");
                throw;
            }
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (await _userRepository.GetUserByUsernameAsync(request.Name) != null)
                    return false;

                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == request.Role);
                if (role == null)
                    throw new ArgumentException($"Role '{request.Role}' does not exist.");

                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    RoleId = role.RoleId,
                    RoleName = role.RoleName
                };

                await _userRepository.AddUserAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration.");
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return string.Equals(hashOfInput, storedHash);
        }
    }
}
