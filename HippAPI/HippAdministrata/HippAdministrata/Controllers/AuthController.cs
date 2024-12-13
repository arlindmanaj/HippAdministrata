﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HippAdministrata.Models.Domains;
using HippAdministrata.Services.Interface;
using HippAdministrata.Models.Requests;
using HippAdministrata.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Data;
using System.Text;
using System.Security.Cryptography;


namespace hippserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public AuthController(IJwtService jwtService, ILogger<AuthController> logger, ApplicationDbContext dbContext)
        {
            _jwtService = jwtService;
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                _logger.LogInformation("Login attempt for user {Username}", model.Name);
                var token = await _jwtService.AuthenticateAsync(model);
                if (token == null)
                {
                    _logger.LogWarning("Login failed for user {Username}", model.Name);
                    return Unauthorized();
                }
                _logger.LogInformation("Login successful for user {Username}", model.Name);

                _logger.LogInformation("Generated JWT Token: {Token}", token);

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user {Username}", model.Name);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Register attempt for user {Username}", request.Name);
                var result = await _jwtService.RegisterAsync(request);
                if (!result)
                {
                    _logger.LogWarning("Registration failed for user {Username}", request.Name);
                    return BadRequest(new { message = "User registration failed." });
                }

                _logger.LogInformation("Registration successful for user {Username}", request.Name);
                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for user {Username}", request.Name);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register/driver")]
        public async Task<IActionResult> RegisterDriver([FromBody] RegisterDriverRequest request)
        {
            try
            {
                _logger.LogInformation("Registering driver {Username}", request.Name);

                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "Driver");
                if (role == null)
                    return BadRequest(new { message = "Driver role not found." });

                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    RoleId = role.RoleId,
                    RoleName = role.RoleName
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var driver = new Driver
                {
                    Name = request.Name,
                    Password = HashPassword(request.Password),
                    LicensePlate = request.LicensePlate,
                    CarModel = request.CarModel,
                    UserId = user.UserId
                };

                await _dbContext.Drivers.AddAsync(driver);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Driver {Username} registered successfully", request.Name);
                return Ok(new { message = "Driver registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during driver registration.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}