using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using qr_backend.Helpers;
using qrbackend.Api.Repository;
using qrbackend.Models.Enums;
using qrbackend.Models.ViewModels;
using qrbackend.Models.ViewModels.Front;
using qrbackend.Models.ViewModels.KeyCard;
using qrbackend.Models.ViewModels.Login;
using qrbackend.Models.ViewModels.Token;
using System;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace qr_backend.Controllers
{

    /// <summary>
    /// Api validacion, registro y authenticacion de clientes.
    /// </summary>
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiController]
    [Authorize]
    public class AuthenticationController : Controller
    {
        readonly IRepositoryAuthentication<PersonalLogin> _authPersonalRepository;
        readonly IRepositoryAuthentication<EmpresarialLogin> _authEmpresarialRepository;

        // GET: /<controller>/
        public AuthenticationController(
            IRepositoryAuthentication<PersonalLogin> authPersonalRepository,
            IRepositoryAuthentication<EmpresarialLogin> authEmpresarialRepository)
        {
            _authPersonalRepository = authPersonalRepository;
            _authEmpresarialRepository = authEmpresarialRepository;
        }


        /// <summary>
        /// Metodo de authenticación usuario empresas y usuario personas
        /// </summary>
        /// <remarks>
        /// Retorna jwtToken y RefrehsToken para loguearse en el sistema
        /// </remarks>
        /// <param name="login"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpPost("Login")]
        //[ResponseCache("Cache-Control" = "no-cache")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(FirstAccesToken), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(502)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login(Login login)
        {
            JwtToken token;

            try
            {
                if (login.Channel == Channel.Enterprise)
                {
                    token = _authEmpresarialRepository.Loggin(new EmpresarialLogin()
                    {
                        DocumentType = ((char)login.Type).ToString(),
                        UserName = login.User,
                        PassWord = login.PassWord,
                        DeviceId = login.DeviceId
                    }
                    , "Empresarial");
                }
                else
                {
                    token = _authPersonalRepository.Loggin(new PersonalLogin()
                    {
                        UserName = login.User,
                        PassWord = login.PassWord,
                        DeviceId = login.DeviceId

                    }
                   , "Personal");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }


            if (token.Code == Enums.GetEnumDescription(ResponseCode.Fail))
            {
                return BadRequest(new FrontStatusCode(Enums.GetEnumDescription(ApiMessages.InvalidUserAndPassWord)));
            }

            if (string.IsNullOrEmpty(token.Token))
            {
                return StatusCode(502, Enums.GetEnumDescription(ApiMessages.BrokerNoResponse));
            }

            await Task.CompletedTask;
            return Ok(new FirstAccesToken()
            {
                FirstTimeAccess = string.IsNullOrEmpty(token.isFirstLogin) ? false : bool.Parse(token.isFirstLogin),
                Token = token.Token,
                RefreshToken = token.RefreshToken
            });
        }

        /// <summary>
        /// Metodo para recuperar coordenada de tarjetas de claves
        /// </summary>
        /// <remarks>
        /// Retorna el número de la coordenada de la tarjeta de claves
        /// </remarks>
        /// <returns>Coordenaa tarjeta de claves </returns>
        /// <response code="200"></response>
        [HttpGet("KeyCard")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(KeyCardPosition), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetKeyCard()
        {
            Coordenate coordenate;
            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];
                var type = string.IsNullOrEmpty(jwt.DocumentType) ? UserType.Personal : UserType.Empresarial;

                coordenate = await _authEmpresarialRepository.GetKeyCardCoordenate(type);

                if (string.IsNullOrEmpty(coordenate.positionNumber))
                {
                    return Unauthorized(new FrontStatusCode(coordenate.Messages));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            return Ok(new KeyCardPosition()
            {
                Position = coordenate.positionNumber
            });
        }

        /// <summary>
        /// Metodo para validar la coordenada de tarjetas de claves y la posición
        /// </summary>
        /// <remarks>
        /// Retorna true o false
        /// </remarks>
        /// <returns>Coordenaa tarjeta de claves </returns>
        /// <response code="200"></response>
        [HttpPost("KeyCard")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> ValidatePersonalKeyCard(ValidateKeyCard keyCard)
        {
            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];
                var type = string.IsNullOrEmpty(jwt.DocumentType) ? UserType.Personal : UserType.Empresarial;

                Coordenate coordenate  = await _authPersonalRepository.GetKeyCardCoordenate(type);
                

                var brokerResponse = _authPersonalRepository.ValidateKeyCard(UserType.Personal, new KeyCardBase()
                {
                    KeyCardCoordinate = coordenate.positionNumber,
                    KeyCardValue = keyCard.Value,
                    UserName = jwt.UserName
                });

                if (string.IsNullOrEmpty(brokerResponse.validated.ToString()))
                {
                    return StatusCode(502, Enums.GetEnumDescription(ApiMessages.BrokerNoResponse));
                }

                if (brokerResponse.validated == false)
                {
                    return BadRequest(new FrontStatusCode("Valor de la tarjéta de claves incorrecto"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok();
        }

        /// <summary>
        /// Metodo para obtener un nuevo token a partir del refresh Token
        /// </summary>
        /// <remarks>
        /// Retorna un nuevo token y un nuevo refresh Token
        /// </remarks>
        /// <returns>Coordenaa tarjeta de claves </returns>
        /// <response code="200"></response>
        [HttpPost("RefreshToken")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(JwtTokenResponse), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RefreshToken(SaveJwtToken token)
        {
            try
            {
                var response = _authPersonalRepository.RefreshToken(token);

                if (string.IsNullOrEmpty(response.Token))
                {
                    return BadRequest(new FrontStatusCode("Refresh Token no pudo ser generado"));
                }

                await Task.CompletedTask;
                return Ok(new JwtTokenResponse()
                {
                    RefreshToken = response.RefreshToken,
                    Token = response.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }
        }
    }
}
