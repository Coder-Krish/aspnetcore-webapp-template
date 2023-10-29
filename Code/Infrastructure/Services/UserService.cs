using Application.Common.dto;
using Application.Common.DTOs;
using Infrastructure.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class UserService
    {
        public static UserList_DTO GetUserDetails(ApplicationDbContext applicationDbContext, UserLoginRequest_DTO userLoginRequest)
        {
            var user = applicationDbContext.Users.Where(a => a.UserName == userLoginRequest.UserName && a.Password == userLoginRequest.Password && a.IsActive == true)
                                        .Select(s => new UserList_DTO
                                        {
                                            UserName = s.UserName,
                                            FullName = s.FullName,
                                            IsActive = s.IsActive,
                                            Role = s.Role,
                                            UserId = s.UserId
                                        }).FirstOrDefault();

            return user;
        }

        public async static Task<string> GenerateJwtToken(UserList_DTO user, IConfiguration _configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Tokens:JwtKey"]);

            var claims = new List<Claim>();
            claims.Add(new Claim("UserName", user.UserName));
            claims.Add(new Claim("FullName", user.FullName));

            var roleString = user.Role;
            string[] roles;
            if (roleString is not null && roleString.Contains(","))
            {
                roles = roleString.Split(',');
            }
            else
            {
                roles = new string[1] { roleString };
            }
            // Add roles as multiple claims
            foreach (var role in roles)
            {
                claims.Add(new Claim("Roles", role));
            }

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
