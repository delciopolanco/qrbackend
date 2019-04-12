using qrbackend.Models.Enums;
using qrbackend.Models.ViewModels;
using qrbackend.Models.ViewModels.Front;
using qrbackend.Models.ViewModels.KeyCard;
using qrbackend.Models.ViewModels.Login;
using qrbackend.Models.ViewModels.Token;
using System.Security.Claims;
using System.Threading.Tasks;

namespace qrbackend.Api.Repository
{
    public interface IRepositoryAuthentication<T> where T: LoginModelBase
    {
        JwtToken RefreshToken(SaveJwtToken token);
        JwtToken Loggin(T login, string rol);
        Task<Coordenate> GetKeyCardCoordenate(UserType type);
        Task<Coordenate> GetKeyCardCoordenateFromCache(UserType type);
        GenericResponse ValidateKeyCard(UserType type, KeyCardBase keyCardModel);
        GenericResponse ForgotPassWord(UserType type, KeyCardBase keyCardModel);
        GenericResponse Register(T newUser);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        FrontStatusCode ValidateGeneralKeyCard(KeyCardBase keycard, UserType type);
    }
}
