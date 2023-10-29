using Application.Common.dto;

namespace Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateUser(UserLoginRequest_DTO userLoginRequest);
    }
}
