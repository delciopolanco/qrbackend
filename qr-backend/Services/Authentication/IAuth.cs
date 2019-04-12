using qrbackend.Models.Entities;
using qrbackend.Models.Enums;
using qrbackend.Models.ViewModels;
using qrbackend.Models.ViewModels.Front;
using qrbackend.Models.ViewModels.KeyCard;
using qrbackend.Models.ViewModels.Login;
using qrbackend.Models.ViewModels.Token;
using System;
using System.Security.Claims;

namespace qrBackend.Api.Services.Authentication
{
    public interface IAuth<T> where T : LoginModelBase
    {

        void GenerateRefreshAndUpdateUser(T user, Action<string> refreshHandler);

        JwtToken CreateTokenWithLogin(T login, string Role);

        JwtToken CreateToken(SaveJwtToken tpkenModel, string Role);

        GenericResponse ValidateKeyCard(UserType type, KeyCardBase keyCardModel);

        GenericResponse ForgotPassWord(UserType type);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
