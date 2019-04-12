using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using qr_backend.Helpers;
using qr_backend.Util;
using qrbackend.Api.Services.BrokerHelper;
using qrbackend.Models.Entities;
using qrbackend.Models.Enums;
using qrbackend.Models.Products;
using qrbackend.Models.RequestBroker;
using qrbackend.Models.ViewModels;
using qrbackend.Models.ViewModels.Beneficiary;
using qrbackend.Models.ViewModels.Front;
using qrbackend.Models.ViewModels.Front.Client;
using qrbackend.Models.ViewModels.Generic;
using qrbackend.Models.ViewModels.Product;

namespace qrbackend.Api.Controllers
{
    /// <summary>
    /// Manejador de Acciones del cliente
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ClientController : Controller
    {
        private IBroker _broker;

        /// <summary>
        /// Contructor de la clase
        /// </summary>
        /// <param name="broker"></param>
        public ClientController(IBroker broker)
        {
            _broker = broker;
        }

        /// <summary>
        /// Metodo para obtener el listado de beneficiarios
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet("Beneficiary")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(List<BeneficiaryFrontList>), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> GetBenericiairyList()
        {
            List<BeneficiaryFrontList> beneficiaryResponse = new List<BeneficiaryFrontList>();

            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];

                var beneficiaryList = _broker.GetFromBroker<GetBeneficiaryList>(new GetBeneficiaryList()
                {
                    DocumentId = jwt.UserName
                });

                if (beneficiaryList == null && string.IsNullOrEmpty(beneficiaryList.CodigoError))
                    return StatusCode(502, "Error de comunicacion con los sistemas externos.");

                if (beneficiaryList.CodigoError != Enums.GetEnumDescription(ResponseCode.Success))
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(beneficiaryList.DescripcionError) ? beneficiaryList.DescripcionError : "Hubo un inconveniente al tratar de recuperar los beneficiarios"));

                foreach (var beneficiary in beneficiaryList.Data)
                {
                    beneficiaryResponse.Add(new BeneficiaryFrontList()
                    {
                        Id = beneficiary.beneficiaryId.ToString(),
                        Image = beneficiary.photoImage,
                        Name = beneficiary.name
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(beneficiaryResponse);
        }

        /// <summary>
        /// Metodo para obtener un beneficiario y sus productos asociados
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet("Beneficiary/{id}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(Beneficiary), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> GetBenericiairy(string id)
        {
            Beneficiary beneficiaryResponse = new Beneficiary();

            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];

                var response = _broker.GetFromBroker<GetBeneficiary>(new GetBeneficiary()
                {
                    DocumentId = jwt.UserName,
                    BeneficiaryId = id
                });

                if (response == null || string.IsNullOrEmpty(response.CodigoError))
                    return StatusCode(502, "Error de comunicacion con los sistemas externos.");

                if (response.CodigoError != Enums.GetEnumDescription(ResponseCode.Success))
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(response.DescripcionError) ? response.DescripcionError : "Hubo un inconveniente al tratar de recuperar el beneficiario"));


                var paymeId = response.Data.ListBeneficiaryProducts != null ? response.Data.ListBeneficiaryProducts.FirstOrDefault(p => p.beneficiaryProductTypeId == (int)ProductType.PaymeId) : new BeneficiaryProduct();
                var phones = response.Data.ListBeneficiaryProducts != null ? response.Data.ListBeneficiaryProducts.Where(m =>
                   m.beneficiaryProductTypeId == (int)ProductType.Phone &&
                   m.beneficiaryProductTypeId != (int)ProductType.PaymeId).Select(x => new BeneficiaryProduct()
                   {
                       beneficiaryId = x.beneficiaryProductId,
                       value = x.value,
                       beneficiaryProductTypeId = x.beneficiaryProductTypeId

                   }).ToList() : new List<BeneficiaryProduct>();
                var products = response.Data.ListBeneficiaryProducts != null ? response.Data.ListBeneficiaryProducts.Where(m =>
                   m.beneficiaryProductTypeId != (int)ProductType.Phone &&
                   m.beneficiaryProductTypeId != (int)ProductType.PaymeId).Select(x => new BeneficiaryProduct()
                   {
                       beneficiaryId = x.beneficiaryProductId,
                       value = x.value,
                       beneficiaryProductTypeId = x.beneficiaryProductTypeId
                   }).ToList() : new List<BeneficiaryProduct>();


                beneficiaryResponse = new Beneficiary()
                {
                    Id = response.Data.beneficiaryId.ToString(),
                    Image = response.Data.photoImage,
                    Name = response.Data.name,
                    PaymeId = paymeId != null ? paymeId.value : string.Empty,
                    Phones = phones != null ? phones.Select(x => new BeneficiaryProducts()
                    {
                        Id = x.beneficiaryProductId,
                        Product = x.value,
                        ProductType = (ProductType)x.beneficiaryProductTypeId
                    }).ToList() : new List<BeneficiaryProducts>(),
                    Products = products != null ? products.Select(x => new BeneficiaryProducts()
                    {
                        Id = x.beneficiaryProductId,
                        Product = x.value,
                        ProductType = (ProductType)x.beneficiaryProductTypeId
                    }).ToList() : new List<BeneficiaryProducts>()
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(beneficiaryResponse);
        }

        /// <summary>
        /// Metodo para guardar el beneficiario
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpPost("Beneficiary")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SaveBeneficiary(AddBeneficiary beneficiary)
        {

            try
            {
                var response = await SaveBeneficiaryBroker(beneficiary);

                if (response.CodigoError == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(response.DescripcionError) ? response.DescripcionError : "Hubo un inconveniente al tratar de guardar el beneficiario"));
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
        /// Metodo para actualizar el beneficiario
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpPut("Beneficiary/{id}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateBeneficiary(UpdateBeneficiary beneficiary)
        {

            try
            {
                var response = await SaveBeneficiaryBroker(beneficiary);

                if (response.CodigoError == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(response.DescripcionError) ? response.DescripcionError : "Hubo un inconveniente al tratar de guardar el beneficiario"));
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
        /// Metodo para eliminar el beneficiario
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpDelete("Beneficiary/{id}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteBeneficiary(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new FrontStatusCode("Debes especificar el Identificador del beneficiario."));
            }

            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];

                var response = _broker.SendBroker<DeleteBeneficiary>(new DeleteBeneficiary()
                {
                    BeneficiaryId = id
                });

                if (response.CodigoError != Enums.GetEnumDescription(ResponseCode.Success))
                {
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(response.DescripcionError) ? response.DescripcionError : "Hubo un inconveniente al tratar de guardar el beneficiario"));
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
        /// Metodo para retornar los productos de un cliente
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet("Products")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> GetProducts()
        {
            List<ProductView> products = new List<ProductView>();

            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];

                var productResponse = _broker.GetFromBroker<GetProductsByDocument>(new GetProductsByDocument()
                {
                    DocumentId = jwt.UserName
                });

                foreach (var product in productResponse.Data)
                {
                    products.Add(new ProductView()
                    {
                        id = product.ProductNumber,
                        balance = !string.IsNullOrEmpty(product.AvailableBalance) ? product.AvailableBalance.Trim() : "",
                        isDefault = product.IsFavorite,
                        currency = product.Currency,
                        currencySymbol = Utilitary.GetCurrencySymbol(product.Currency),
                        type = Utilitary.GetProductTypeIdByDescription(product.ProducType),
                        alias = product.Alias,
                        entity = product.ProviderName,
                        name = product.Name,
                        subtype = product.Subtype,
                        message = product.Message
                    });
                }
                

                if (productResponse.CodigoError == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode("Hubo un inconveniente al obtener la respesta de los productos"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(products);
        }

        private async Task<AddOrUpdateBeneficiary> SaveBeneficiaryBroker(dynamic beneficiary)
        {
            JwtData jwt = (JwtData)RouteData.Values["jwtData"];

            var _beneficiary = new AddOrUpdateBeneficiary()
            {
                DocumentId = jwt.UserName,
                FullName = beneficiary.Name,
                BeneficiaryProducts = SplitBeneficiaryProducts(beneficiary.PaymeId, beneficiary.Phones, beneficiary.Products)
            };

            if (StringHelper.IsPropertyExist(beneficiary, "Id"))
            {
                _beneficiary.BeneficiaryId = beneficiary.Id;
            }

            await Task.CompletedTask;
            return _broker.SendBroker<AddOrUpdateBeneficiary>(_beneficiary);
        }

        private string SplitBeneficiaryProducts(string paymeId, List<AddBeneficiaryProducts> phones, List<AddBeneficiaryProducts> products)
        {
            var beneficiaries = string.Empty;
            string _paymeId = !string.IsNullOrEmpty(paymeId) ? $"{(int)ProductType.PaymeId};{paymeId}|" : "";

            foreach (var p in products)
                beneficiaries = string.Concat(beneficiaries, $"{(int)p.ProductType};{p.Product}|");

            foreach (var p in phones)
                beneficiaries = string.Concat(beneficiaries, $"{(int)p.ProductType};{p.Product}|");

            return string.Concat(_paymeId, beneficiaries.Remove(beneficiaries.Length - 1));
        }


        /// <summary>
        /// Metodo para recuperar la info asociada a cuentas y tarjetas.
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet("Account/{accountNumber}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(ClientValidationProduct), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> GetAccountInfo(string accountNumber)
        {
            ClientValidationProduct product = new ClientValidationProduct();

            if(string.IsNullOrEmpty(accountNumber))
            {
                return BadRequest(new FrontStatusCode("Debes especificar una cuenta para realizar la consulta"));
            }

            try
            {
                
                var genericRequest = new GetGeneric<BeneficiaryAccount>(GenericEndPoints.BeneficiaryValidateAccount, accountNumber);

                var response = _broker.SendBroker<MQResponse4<BeneficiaryAccount>>(genericRequest);

                
                if (response.CodigoError == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(response.DescripcionError) ? response.DescripcionError : "Hubo un inconveniente al recuperar los datos de la cuenta."));
                }

                product = new ClientValidationProduct()
                {
                    ClientName = response.Data.FullName,
                    Image = response.Data.PhotoImge,
                    ProductType = Enums.GetValueFromDescription<ProductType2>(response.Data.Type),
                    Status = response.Data.Status
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(product);
        }

        /// <summary>
        /// Metodo para recuperar la info al PaymeId.
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet("PaymeId/{paymeId}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(ClientValidationProduct), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> GetPaymeIdInfo(string paymeId)
        {
            ClientValidationProduct product = new ClientValidationProduct();

            if (string.IsNullOrEmpty(paymeId))
            {
                return BadRequest(new FrontStatusCode("Debes especificar el paymeId para realizar la consulta"));
            }

            try
            {

                var genericRequest = new GetGeneric<BeneficiaryAccount>(GenericEndPoints.GetByPaymeId, paymeId);

                var response = _broker.SendBroker<MQResponse4<BeneficiaryAccount>>(genericRequest);


                if (response.CodigoError == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(response.DescripcionError) ? response.DescripcionError : "Hubo un inconveniente al recuperar los datos de la cuenta."));
                }

                product = new ClientValidationProduct()
                {
                    ClientName = response.Data.FullName,
                    Image = response.Data.PhotoImge,
                    Status = response.Data.Status
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(product);
        }

        /// <summary>
        /// Metodo para guardar imagen del Cliente
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpPost("SaveImage")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SaveImage(SaveImage saveImage)
        {

            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];

                SaveImageBroker saveImageBroker = new SaveImageBroker {
                    EndPoint = "client/saveImage",
                    FunctionName = Utilitary.postGenericBroker,
                    documentId = jwt.UserName,
                    documentType = jwt.DocumentType

                };

                var saveImageResponse = _broker.SendBroker<JsonResponseGeneric>(saveImageBroker);

                if (saveImageResponse.CodigoError == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(saveImageResponse.DescripcionError) ? saveImageResponse.DescripcionError : "Hubo un inconveniente al tratar de actualizar la imagen del cliente"));
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
        /// Metodo para guardar imagen del Cliente
        /// </summary>
        /// <remarks>
        /// Retorna http response 200
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpPost("SavingsPlan")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SavingsPlan(SavingPlan savingPlan)
        {

            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];
                var mapper = Utilitary.CreateMapper<SavingPlan, SavingPlanRequestBroker>();

                var savingMapped = mapper(savingPlan);
                savingMapped.EndPoint = "client/saveSavingsPlan";
                savingMapped.FunctionName = Utilitary.postGenericBroker;
                savingMapped.documentId = jwt.UserName;
                savingMapped.documentType = jwt.DocumentType;
                
                var saveImageResponse = _broker.SendBroker<JsonResponseGeneric>(savingMapped);

                if (saveImageResponse.CodigoError == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(saveImageResponse.DescripcionError) ? saveImageResponse.DescripcionError : "Hubo un inconveniente al tratar de actualizar el plan de ahorro del cliente"));
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok();
        }

        [HttpGet("GetSavingsPlan")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(typeof(TransactionQrCodeResp), 200)]
        [ProducesResponseType(typeof(TransactionQrCodeResp), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> GetSavingsPlan()
        {
            SavingPlan savingPlan;
            try
            {
                JwtData jwt = (JwtData)RouteData.Values["jwtData"];

                var responseSavingPlan = _broker.SendBroker<JsonRequestGeneric>(
                    new JsonRequestGeneric("GetGeneric")
                    {
                        EndPoint = "client/saveSavingsPlan?documentId="+ jwt.UserName +"&" + jwt.DocumentType
                    }
                );

                if (responseSavingPlan == null && string.IsNullOrEmpty(responseSavingPlan.CodigoError))
                    return StatusCode(502, "Error de comunicacion con los sistemas externos.");

                if (responseSavingPlan.CodigoError != Enums.GetEnumDescription(ResponseCode.Success))
                    return BadRequest(new FrontStatusCode(!string.IsNullOrEmpty(responseSavingPlan.DescripcionError) ? responseSavingPlan.DescripcionError : "Hubo un inconveniente al tratar de obtener el plan de ahorro de un cliente"));


            savingPlan = JsonConvert.DeserializeObject<SavingPlan>(responseSavingPlan.Data.ToString());

            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            await Task.CompletedTask;
            return Ok(savingPlan);
        }
    }
}