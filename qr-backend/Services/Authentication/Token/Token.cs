using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using qrbackend.Models.Entities;
using qrbackend.Models.ViewModels.Token;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace qrbackend.Api.Services.Authentication.Token
{
    public class Token : IToken
    {
        readonly IConfiguration _config;

        public Token(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Issuer"],
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public string GetRefreshToken(Entity UserInDb)
        {
            return ((User)UserInDb).RefreshToken;
        }

        public async Task<Entity> GetUserInDb(JwtToken tokenViewModel, string username)
        {
            await Task.CompletedTask;
            return new User();//_context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task UpdateRefreshToken(Entity UserInDb, string newRefreshToken)
        {
            ((User)UserInDb).RefreshToken = newRefreshToken;
            // _context.Users.Update(((User)UserInDb));

            await Task.CompletedTask; //_context.SaveChangesAsync();
        }
    }
}
