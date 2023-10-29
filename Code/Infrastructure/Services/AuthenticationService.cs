using Application.Common.dto;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticationService(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<object> AuthenticateUser(UserLoginRequest_DTO userLoginRequest)
        {
            var identityUser = await _userManager.FindByEmailAsync(userLoginRequest.UserName);
            if(identityUser == null)
            {
                return false;
            }
            if(await _userManager.CheckPasswordAsync(identityUser, userLoginRequest.Password))
            {
                return identityUser;
            }
            return false;
        }

        public async Task<bool> RegisterUser(UserRegistration_DTO user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            return result.Succeeded;
        }

        public async Task<string> GenerateTokenString(object user)
        {
            var identityUser = (IdentityUser)user;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Tokens:JwtKey"]);

            var claims = new List<Claim>();
            claims.Add(new Claim("UserName", identityUser.UserName));
            claims.Add(new Claim("Email", identityUser.Email));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Tokens:JwtValidMinutes"])),
                Issuer = _configuration["Tokens:JwtIssuer"],
                Audience = _configuration["Tokens:JwtAudience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
