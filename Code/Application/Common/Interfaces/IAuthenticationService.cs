using Application.Common.dto;
using Application.Common.DTOs;

namespace Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        Task<object> AuthenticateUser(UserLoginRequest_DTO userLoginRequest);
        Task<bool> RegisterUser(UserRegistration_DTO user);
        Task<string> GenerateTokenString(object user);
    }
}
