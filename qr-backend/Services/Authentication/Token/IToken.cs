using qrbackend.Models.Entities;
using qrbackend.Models.ViewModels.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace qrbackend.Api.Services.Authentication.Token
{
    public interface IToken
    {
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GetRefreshToken(Entity UserInDb);
        Task UpdateRefreshToken(Entity UserInDb, string newRefreshToken);
        Task<Entity> GetUserInDb(JwtToken jwtToken, string username);
    }
}
