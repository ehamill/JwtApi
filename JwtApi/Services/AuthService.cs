using JwtApi.Data;
using JwtApi.Entities;
using JwtApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtApi.Services
{
    public class AuthService(UserDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<User?> RegisterAsync(UserDto request)
        {
            try {
                if (await context.Users.AnyAsync(c => c.UserName == request.UserName))
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            var user = new User();
            var hashedPassword = new PasswordHasher<User>() // request.Password;
                .HashPassword(user, request.Password);

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.PasswordHash = hashedPassword;
            user.UserId = Guid.NewGuid().ToString();

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }
        public async Task<string> LoginAsync(UserDto request) {

            var user = await context.Users.FirstOrDefaultAsync(c => c.UserName == request.UserName);
            if (user == null) {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return CreateToken(user);

        }

        


        private string CreateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            //install nuget ""system.identityModel.tokens.jwt
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512); // requires 64 chars

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            //returns encoded data.. go to www.jwt.io to decode the data
        }


    }
}
