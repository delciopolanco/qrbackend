
using IBM.WMQ;
using qrbackend.Api.Services.Authentication.Security;
using qrbackend.Api.Services.Authentication.Token;
using qrbackend.Api.Services.BrokerHelper;
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
    public class Auth<T> : IAuth<T> where T : LoginModelBase
    {

        readonly IToken tokenService;
        readonly ISecurity securityService;
        readonly IBroker broker;

        public Auth(IToken _tokenService, ISecurity _securityHandler, IBroker _broker)
        {
            tokenService = _tokenService;
            securityService = _securityHandler;
            broker = _broker;
        }

        public void GenerateRefreshAndUpdateUser(T login, Action<string> refreshHandler)
        {
            var refreshToken = tokenService.GenerateRefreshToken();


            JwtRefreshToken tokenResponse = broker.SendBroker<JwtRefreshToken>(new JwtRefreshToken()
            {
                RefreshToken = refreshToken,
                UserName = login.UserName
            });

            refreshHandler.Invoke(!string.IsNullOrEmpty(tokenResponse.Code) ? refreshToken : string.Empty);
            //  refreshHandler.Invoke(refreshToken);
        }

        public JwtToken CreateTokenWithLogin(T login, string Role)
        {

            JwtToken jwtToken = new JwtToken();

            try
            {
                var mqResponse = broker.SendBroker<T>(login);

                if (mqResponse.Code != Enums.GetEnumDescription(ResponseCode.Success))
                {
                    jwtToken.Code = mqResponse.Code;
                    jwtToken.Messages = mqResponse.Messages;
                    return jwtToken;
                }

                jwtToken.isFirstLogin = mqResponse.isFirstLogin.ToString();

                GenerateRefreshAndUpdateUser(login, (_rt) =>
                {
                    if (string.IsNullOrEmpty(_rt))
                    {
                        throw new Exception("Refresh Token no pudo ser generado");
                    }

                    jwtToken.RefreshToken = _rt;
                });


                jwtToken.Token = tokenService.GenerateToken(new Claim[]
                    {
                        new Claim(ClaimTypes.Role, Role),
                        new Claim("UserName", login.UserName),
                        new Claim("DocumentType", !string.IsNullOrEmpty(login.DocumentType) ? login.DocumentType : string.Empty)
                    });

            }
            catch (MQException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jwtToken;
        }

        public GenericResponse ValidateKeyCard(UserType type, KeyCardBase keyCardModel)
        {
            GenericResponse genericResponse = new GenericResponse();
            MQResponseResult mqResponse;

            try
            {
                if (type == UserType.Personal)
                {
                    keyCardModel.FunctionName = "ValidatePersonalKeyCard";
                    mqResponse = broker.SendBroker<MQResponseResult>(keyCardModel);
                }
                else
                {
                    keyCardModel.FunctionName = "ValidateEmpresarialKeyCard";
                    mqResponse = broker.SendBroker<MQResponseResult>(keyCardModel);
                }

                genericResponse.validated = (mqResponse.Code == Enums.GetEnumDescription(ResponseCode.Success));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return genericResponse;
        }

        public GenericResponse ForgotPassWord(UserType type)
        {
            return null;
        }

        public JwtToken CreateToken(SaveJwtToken token, string Role)
        {
            JwtToken jwtToken = new JwtToken();

            try
            {
                GenerateRefreshAndUpdateUser(token, (_rt) =>
                {
                    if (string.IsNullOrEmpty(_rt))
                    {
                        throw new Exception("Refresh Token no pudo ser generado");
                    }

                    jwtToken.RefreshToken = _rt;
                });


                jwtToken.Token = tokenService.GenerateToken(new Claim[]
                    {
                        new Claim(ClaimTypes.Role, Role),
                        new Claim(ClaimTypes.Name, token.UserName)
                    });
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return jwtToken;
        }

        private void GenerateRefreshAndUpdateUser(SaveJwtToken login, Action<string> refreshHandler)
        {
            var refreshToken = tokenService.GenerateRefreshToken();


            JwtRefreshToken tokenResponse = broker.SendBroker<JwtRefreshToken>(new JwtRefreshToken()
            {
                RefreshToken = refreshToken,
                UserName = login.UserName
            });

            refreshHandler.Invoke(!string.IsNullOrEmpty(tokenResponse.Code) ? refreshToken : string.Empty);
            refreshHandler.Invoke(refreshToken);

        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            return tokenService.GetPrincipalFromExpiredToken(token);
        }
    }
}
