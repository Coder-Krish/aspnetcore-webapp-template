using Application.Common.dto;
using Application.Common.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationService(ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
        }
        public async Task<string> AuthenticateUser(UserLoginRequest_DTO userLoginRequest)
        {
            var userDetail = UserService.GetUserDetails(_applicationDbContext, userLoginRequest);
            if (userDetail == null)
            {
                throw new Exception("User Not Found!!");
            }
            else
            {
                var token = await UserService.GenerateJwtToken(userDetail, _configuration);
                return token;
            }
        }
    }
}
