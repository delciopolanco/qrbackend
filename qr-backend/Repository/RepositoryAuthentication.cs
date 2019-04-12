using qrbackend.Models.ViewModels.Token;
using qrbackend.Models.ViewModels.Login;
using qrBackend.Api.Services.Authentication;
using qr_backend.Services.Cache;
using System;
using qrbackend.Models.ViewModels.KeyCard;
using qrbackend.Models.Enums;
using qrbackend.Models.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;
using qrbackend.Models.ViewModels.Front;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace qrbackend.Api.Repository
{
    public class RepositoryAuthentication<T> : IRepositoryAuthentication<T> where T : LoginModelBase
    {
        readonly IAuth<T> _auth;
        readonly ICache _cache;

        public RepositoryAuthentication() { }

        public RepositoryAuthentication(IAuth<T> auth, ICache cache)
        {
            _auth = auth;
            _cache = cache;
        }

        public async Task<Coordenate> GetKeyCardCoordenate(UserType type)
        {
            Coordenate coordenate = new Coordenate();

            try
            {
                if(type == UserType.Personal)
                    coordenate.positionNumber = (string)_cache.Get("coordenate");
                else
                    coordenate.positionNumber = (string)_cache.Get("coordenate-empresarial");

                if (!string.IsNullOrEmpty(coordenate.positionNumber))
                    return new Coordenate(coordenate.positionNumber);

                coordenate.positionNumber = RandomNumber(1, 40, type);

            }
            catch (Exception ex)
            {
                coordenate.Messages = ex.Message;
            }

            return coordenate;
        }

        public async Task<Coordenate> GetKeyCardCoordenateFromCache(UserType type)
        {
            Coordenate coordenate = new Coordenate();

            try
            {
                if (type == UserType.Personal)
                    coordenate.positionNumber = (string)_cache.Get("coordenate");
                else
                    coordenate.positionNumber = (string)_cache.Get("coordenate-empresarial");

                if (string.IsNullOrEmpty(coordenate.positionNumber))
                    return new Coordenate();
            }
            catch (Exception ex)
            {
                coordenate.Messages = ex.Message;
            }

            return coordenate;
        }

        public JwtToken Loggin(T login, string rol)
        {
            return _auth.CreateTokenWithLogin(login, rol);
        }

        public JwtToken RefreshToken(SaveJwtToken token)
        {
            var principal = _auth.GetPrincipalFromExpiredToken(token.Token);
            var ci = principal as ClaimsPrincipal;
            var claim = ci.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

            string rol = claim.Value;

            token.UserName = principal.Identity.Name;

            return _auth.CreateToken(token, rol);
        }

        public GenericResponse Register(T newUser)
        {
            throw new System.NotImplementedException();
        }

        public GenericResponse ForgotPassWord(UserType type, KeyCardBase keyCardModel)
        {
            throw new System.NotImplementedException();
        }

        public GenericResponse ValidateKeyCard(UserType type, KeyCardBase keyCardModel)
        {
            return _auth.ValidateKeyCard(type, keyCardModel);
        }

        public FrontStatusCode ValidateGeneralKeyCard(KeyCardBase keycard, UserType type)
        {
            var statusCode = new FrontStatusCode("Validacion de tarjetas de clave correcta", "00");

            var brokerResponse = ValidateKeyCard(type, keycard);

            if (string.IsNullOrEmpty(brokerResponse.validated.ToString()))
            {
                statusCode = new FrontStatusCode(Enums.GetEnumDescription(ApiMessages.BrokerNoResponse), "502");
            }

            if (brokerResponse.validated == false)
            {
                statusCode = new FrontStatusCode("Valor de la tarjéta de claves incorrecto");
            }


            if (type == UserType.Empresarial)
            {
                _cache.Delete("coordenate-empresarial");
            }
            else
            {
                _cache.Delete("coordenate");
            }

            return statusCode;
        }

        private string RandomNumber(int min, int max, UserType type)
        {
            Random random = new Random();
            string coordenate = random.Next(min, max).ToString();

            if (type == UserType.Empresarial)
            {
                _cache.Add("coordenate-empresarial", coordenate, DateTimeOffset.Now.AddMonths(5));
            }
            else
            {
                _cache.Add("coordenate", coordenate, DateTimeOffset.Now.AddMonths(5));
            }

            return coordenate;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            return null;
        }
    }
}
