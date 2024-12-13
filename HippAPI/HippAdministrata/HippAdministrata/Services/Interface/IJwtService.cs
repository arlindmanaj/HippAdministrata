using HippAdministrata.Models.Responses;
using HippAdministrata.Models.Requests;
using HippAdministrata.Models.DTOs;

namespace HippAdministrata.Services.Interface
{
    public interface IJwtService
    {
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
