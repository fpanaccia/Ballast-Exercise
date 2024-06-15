using Identity.Api.Application.Commands;
using Identity.Domain.Aggregates.User;
using Identity.Domain.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Api.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<string> LoginAttempt(string email, string password)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user is null)
            {
                throw new UserDoesNotExistException();
            }

            if (string.IsNullOrEmpty(password) || user.Password != password) //Here we should be comparing hashes if the password were encrypted
            {
                throw new InvalidPasswordException();
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub, email),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(4), //The token has this long expiration to simplify the exercise
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token); //We should also store the token if it weren't a simplified auth
        }
    }
}
