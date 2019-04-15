using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using qr_backend.Helpers;
using qr_backend.Repository;
using qrbackend.Api.Repository;
using qrbackend.Api.Services.BrokerHelper;
using qrbackend.Models.Entities;
using qrbackend.Models.Enums;
using qrbackend.Models.ViewModels;
using qrbackend.Models.ViewModels.Front;
using qrbackend.Models.ViewModels.Generic;
using qrbackend.Models.ViewModels.KeyCard;
using qrbackend.Models.ViewModels.Login;
using qrbackend.Models.ViewModels.Notification;
using qrbackend.Models.ViewModels.Profile;
using qrbackend.Models.ViewModels.Token;
using System;
using System.Threading.Tasks;

namespace qr_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProfileController : Controller
    {
        readonly IRepositoryAuthentication<PersonalLogin> _authPersonalRepository;
        readonly IBroker _broker;
        readonly IRepository<ValidateToken> _token;


        //   readonly IRepository<SavePaymeId> _profile;

        public ProfileController(
             IBroker broker,
            IRepositoryAuthentication<PersonalLogin> authPersonalRepository,
            IRepository<ValidatePaymeId> profile,
            IRepository<ValidateToken> token)
        {
            _broker = broker;
            _authPersonalRepository = authPersonalRepository;
            _token = token;
        }

        /// <summary>
        /// Metodo para registrar el tipo de notificacion para token digital del usuario
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpPost("Notification")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> Save(SaveNotificationType type)
        {
            try
            {
                var jwtToken = (JwtData)RouteData.Values["jwtData"];
                var userType = string.IsNullOrEmpty(jwtToken.DocumentType) ? UserType.Personal : UserType.Empresarial;

                Coordenate coordenate = await _authPersonalRepository.GetKeyCardCoordenateFromCache(userType);

                if (string.IsNullOrEmpty(coordenate.positionNumber))
                {
                    return BadRequest(new FrontStatusCode("Debes solicitar la coordenada de tarjetas de claves para poder continuar."));
                }

                var keyCardResponseCode = _authPersonalRepository.ValidateGeneralKeyCard(new KeyCardBase()
                {
                    KeyCardCoordinate = coordenate.positionNumber,
                    KeyCardValue = type.KeyCardValue,
                    DeviceId = type.DeviceId,
                    UserName = jwtToken.UserName,
                    DocumentType = jwtToken.DocumentType
                }, userType);

                if (keyCardResponseCode.Code == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(keyCardResponseCode);
                }


                SavePersonalNotificationType personal = new SavePersonalNotificationType();
                SaveEmpresarialNotificationType empresarial = new SaveEmpresarialNotificationType();

                if (jwtToken.Role == Enums.GetEnumDescription(UserType.Personal))
                {
                    personal = new SavePersonalNotificationType()
                    {
                        NotificationConfig = type.TargetDevice,
                        NotificationType = ((int)type.Type).ToString(),
                        UserName = jwtToken.UserName,
                        DeviceId = type.DeviceId
                    };

                }
                else
                {
                    empresarial = new SaveEmpresarialNotificationType()
                    {
                        NotificationConfig = type.TargetDevice,
                        NotificationType = ((int)type.Type).ToString(),
                        UserName = jwtToken.UserName,
                        DeviceId = type.DeviceId
                    };

                }

                var response = (jwtToken.Role == Enums.GetEnumDescription(UserType.Personal)
                    ? _broker.SendBroker<MQResponseResult>(personal)
                    : _broker.SendBroker<MQResponseResult>(empresarial));


                if (response.Code != Enums.GetEnumDescription(ResponseCode.Success))
                    return BadRequest(new FrontStatusCode("Error al registrar el medio de seguridad: " + response.Messages));

            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok();
        }

        /// <summary>
        /// Metodo para validar si el PaymeID existe o ya se encuentra registrado
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet("PaymeId/{id}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByID(string id)
        {
            try
            {
                var response = _broker.SendBroker<ValidatePaymeId>(new ValidatePaymeId()
                {
                    PaymeId = id
                });

                if (response.Code != Enums.GetEnumDescription(ResponseCode.Success))
                {
                    return BadRequest(new FrontStatusCode("El PaymeID se encuentra en uso"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            return Ok();
        }

        /// <summary>
        /// Metodo para registrar el paymeID del usuario
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpPost("PaymeId")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> Save(SavePaymeId savePaymeID)
        {
            try
            {
                var jwtToken = (JwtData)RouteData.Values["jwtData"];

                var tokenResponse = _token.Get(new ValidateToken()
                {
                    UserName = jwtToken.UserName,
                    TransactionToken = savePaymeID.code,
                    DeviceId = savePaymeID.DeviceId
                });

                if (tokenResponse.Code == Enums.GetEnumDescription(ResponseCode.Expired))
                {
                    return BadRequest(new FrontStatusCode("token de transacción expirado"));
                }

                if (tokenResponse.Code == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode("token de transacción inválido"));
                }


                var response = _broker.SendBroker<SavePersonalPaymeId>(new SavePersonalPaymeId()
                {
                    PaymeId = savePaymeID.PayMeID,
                    UserName = jwtToken.UserName
                });

                if (response.Code != Enums.GetEnumDescription(ResponseCode.Success))
                {
                    return BadRequest(new FrontStatusCode("No pudo realizarse el registro del paymeID"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            return Ok();
        }

        /// <summary>
        /// Servicio para obtener el listado de empresas de un usuario
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet("Companies")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(typeof(CompaniesList), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                //var jwtToken = (JwtData)RouteData.Values["jwtData"];

                //var tokenResponse = _token.Get(new ValidateToken()
                //{
                //    UserName = jwtToken.Name,
                //    TransactionToken = savePaymeID.code,
                //    DeviceId = savePaymeID.DeviceId
                //});

                //if (tokenResponse.Code == Enums.GetEnumDescription(ResponseCode.Expired))
                //{
                //    return BadRequest(new FrontStatusCode("token de transacción expirado"));
                //}

                //if (tokenResponse.Code == Enums.GetEnumDescription(ResponseCode.Fail))
                //{
                //    return BadRequest(new FrontStatusCode("token de transacción inválido"));
                //}

            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            return Ok(StringHelper.GetJsonFromFile("companies"));
        }


        /// <summary>
        /// Servicio para recuperar datos del perfil
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet()]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(typeof(Profile), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> Get()
        {
            Profile profile = new Profile();

            try
            {
                JwtData jwtToken = (JwtData)RouteData.Values["jwtData"];

                var genericRequest = new GetGeneric<GenericProfile>(GenericEndPoints.ClientGet, jwtToken.UserName);

                var response = _broker.SendBroker<MQResponse4<GenericProfile>>(genericRequest);


                if (response.CodigoError != Enums.GetEnumDescription(ResponseCode.Success))
                {
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(response.DescripcionError) ? response.DescripcionError : "Hubo un inconveniente al tratar de recuperar el perfil de usuario"));
                }

                profile = new Profile()
                {
                    Email = response.Data.Email,
                    Name = response.Data.FullName,
                    PayMeID = response.Data.PaymeId,
                    Image = response.Data.PhotoImage,
                    ExistInAlternateDomain = false
                };

            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(profile);
        }
    }
}
