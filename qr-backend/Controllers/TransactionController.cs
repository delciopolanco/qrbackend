using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using qr_backend.Helpers;
using qrbackend.Api.Services.BrokerHelper;
using qrbackend.Models.Enums;
using qrbackend.Models.ViewModels;
using qrbackend.Models.ViewModels.Front;
using qrbackend.Models.ViewModels.Generic;
using qrbackend.Models.ViewModels.Token;
using qrbackend.Models.ViewModels.Pinqrcode;
using Newtonsoft.Json;
using qr_backend.Util;
using qrbackend.Models.RequestBroker;

namespace qrbackend.Api.Controllers
{
    /// <summary>
    /// Api validacion, registro y obentecion de transacciones.
    /// </summary>
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiController]
    [Authorize]
    public class TransactionController : Controller
    {
        private IBroker _broker;

        public TransactionController(IBroker broker)
        {
            _broker = broker;
        }

        /// <summary>
        /// Obtencion de un nuevo token de transación.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Token/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(502)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetToken(string DeviceId)
        {
            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];

                var response = _broker.SendBroker<GenerateTransactionToken>(new GenerateTransactionToken()
                {
                    UserName = jwt.UserName,
                    DeviceId = DeviceId
                });

                if (string.IsNullOrEmpty(response.Code))
                {
                    return StatusCode(502, Enums.GetEnumDescription(ApiMessages.BrokerNoResponse));
                }

                if(response.Code == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode("Hubo un error al refrescar el tokén de transacción y/o DeviceId inválido"));
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
        /// Obtiene el listado de transacciones del usuario.
        /// </summary>
        /// <returns></returns>
        [HttpGet("History")]
        [ProducesResponseType(typeof(List<HistoryTransaction>), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(502)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTransactions(string account)
        {

            List<HistoryTransaction> listTransaction = new List<HistoryTransaction>();

            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];

                var responseListTransaction = _broker.SendBroker<JsonRequestGeneric>(
                    new JsonRequestGeneric("GetGeneric")
                    {
                        EndPoint = "transaction/getList?Account=" + account
                    }
                );

                if (responseListTransaction == null && string.IsNullOrEmpty(responseListTransaction.CodigoError))
                    return StatusCode(502, "Error de comunicacion con los sistemas externos.");

                if (responseListTransaction.CodigoError != Enums.GetEnumDescription(ResponseCode.Success))
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(responseListTransaction.DescripcionError) ? responseListTransaction.DescripcionError : "Hubo un inconveniente al tratar de recuperar los beneficiarios"));

                if(responseListTransaction.Data != null) 
                    listTransaction = JsonConvert.DeserializeObject<List<HistoryTransaction>>(responseListTransaction.Data.ToString());

            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(listTransaction);
        }

        [HttpGet("GetQrPin")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(TransactionQrCodeResp), 200)]
        [ProducesResponseType(typeof(TransactionQrCodeResp), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> GetQrPin(string Pin)
        {
            TransactionQrCodeResp trxQr;
            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];

                var responsepinqrcode = _broker.SendBroker<JsonRequestGeneric>(
                    new JsonRequestGeneric("GetGeneric") {
                        EndPoint = "pinqrcode/getByPin?pin="+Pin
                    }
                );

                if (responsepinqrcode == null && string.IsNullOrEmpty(responsepinqrcode.CodigoError))
                    return StatusCode(502, "Error de comunicacion con los sistemas externos.");

                if (responsepinqrcode.CodigoError != Enums.GetEnumDescription(ResponseCode.Success))
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(responsepinqrcode.DescripcionError) ? responsepinqrcode.DescripcionError : "Hubo un inconveniente al tratar de recuperar los beneficiarios"));


                trxQr = JsonConvert.DeserializeObject<TransactionQrCodeResp>(responsepinqrcode.Data.ToString());
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(trxQr);
        }

        [HttpPost("AddQrPin")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(String), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> AddQrPin(TransactionQrCode trxQr)
        {
            string pinqr;
            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];
                var mapper = Utilitary.CreateMapper<TransactionQrCode, TransactionQrCodeBroker>();

                var trxQrMapped = mapper(trxQr);
                trxQrMapped.EndPoint = "pinqrcode/add";
                trxQrMapped.FunctionName = Utilitary.postGenericBroker;

                var responseAddpinqrcode = _broker.SendBroker<JsonResponseGeneric>(trxQrMapped);

                if (responseAddpinqrcode == null && string.IsNullOrEmpty(responseAddpinqrcode.CodigoError))
                    return StatusCode(502, "Error de comunicacion con los sistemas externos.");

                if (responseAddpinqrcode.CodigoError != Enums.GetEnumDescription(ResponseCode.Success))
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(responseAddpinqrcode.DescripcionError) ? responseAddpinqrcode.DescripcionError : "Hubo un inconveniente al tratar de recuperar los beneficiarios"));

                pinqr = JsonConvert.DeserializeObject<string>(responseAddpinqrcode.Data.ToString());

            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(pinqr);
        }

        [HttpPost("Send")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(JsonResponseGeneric), 200)]
        [ProducesResponseType(typeof(JsonResponseGeneric), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> Send(Transaction trx)
        {
            JsonResponseGeneric resultTransactionSend;
            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];
                var mapper = Utilitary.CreateMapper<Transaction, TransactionBroker>();

                var trxMapped = mapper(trx);

                trxMapped.EndPoint = "transaction/send";
                trxMapped.documentIdSender = jwt.UserName;
                trxMapped.documentTypeSender = jwt.DocumentType;
                trxMapped.FunctionName = Utilitary.postGenericBroker; 

                var saveTransaction = _broker.SendBroker<TransactionBroker>(trxMapped);

                if (saveTransaction == null && string.IsNullOrEmpty(saveTransaction.CodigoError))
                    return StatusCode(502, "Error de comunicacion con los sistemas externos.");

                if (saveTransaction.CodigoError != Enums.GetEnumDescription(ResponseCode.Success))
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(saveTransaction.DescripcionError) ? saveTransaction.DescripcionError : "Hubo un inconveniente al tratar de recuperar los beneficiarios"));



                resultTransactionSend = new JsonResponseGeneric {
                    CodigoError = saveTransaction.CodigoError,
                    DescripcionError = saveTransaction.DescripcionError,
                    Data = saveTransaction.Data
                };

            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(resultTransactionSend);
        }
    }


}