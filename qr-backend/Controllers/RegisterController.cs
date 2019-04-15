using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using qrbackend.Models.ViewModels;
using Microsoft.Extensions.FileProviders;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using qrbackend.Models.ViewModels.Front;
using qrbackend.Models.Enums;
using System.Text;
using qr_backend.Helpers;
using qrbackend.Api.Services.BrokerHelper;
using qrbackend.Models.ViewModels.Registration;

namespace qrbackend.Api.Controllers
{
    /// <summary>
    /// Api registro de cliente
    /// </summary>
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
    [ApiController]
    public class RegisterController : Controller
    {

        private IHostingEnvironment _environment;
        private IBroker _broker;

        public RegisterController(IHostingEnvironment environment, IBroker broker)
        {
            _environment = environment;
            _broker = broker;
        }

        /// <summary>
        /// Validacion del registro con Tecnologia OCR
        /// </summary>
        [HttpPost("Document")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(typeof(RegistrationResponse), 200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Validate(IFormFile File, DocumentType Type)
        {
            try
            {

                /*using (var engine = new TesseractEngine(Path.Combine(_environment.WebRootPath, "tessdata"), "eng", EngineMode.Default))
                {
                    using (var image = new Bitmap(File.OpenReadStream()))
                    {
                        using (var page = engine.Process(image))
                        {

                            RegistrationResponse response = new RegistrationResponse();
                            response.MeanConfidence = page.GetMeanConfidence();

                            var text = page.GetText().Split(" ");

                            if (text.Length >= 30)
                            {
                                var name = $"{text[26].Trim()} {text[27].Trim()} {text[28].Trim()}";

                                response.Name = StringHelper.RemoveSpecialCharacters(name).Trim();
                                response.Document = text[31];
                                response.Address = text[29];

                                await Task.CompletedTask;
                                return Ok(response);
                            }

                            return BadRequest(new FrontStatusCode("La imagen capturada posee muy baja resolución"));
                        }

                    }
                }*/
                return null;
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }
        }

        /// <summary>
        /// Registro de nuevos clientes
        /// </summary>
        [HttpPost()]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(FrontStatusCode), 400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> Register(FrontClientRegistration registration )
        {
            try
            {
                var response = _broker.SendBroker<ClientRegistration>(new ClientRegistration()
                {
                    Name = registration.Name,
                    Address = registration.Address,
                    DocumentPicture = registration.DocumentPicture,
                    Selfie = registration.Selfie,
                    ClientType = Enums.GetEnumDescription(registration.ClientType),
                    Contact = registration.Contact
                });

                if(string.IsNullOrEmpty(response.Code))
                {
                    return StatusCode(502, "Error de comunicacion con los sistemas externos.");
                }

                if (response.Code == Enums.GetEnumDescription(ResponseCode.Fail))
                {
                    return BadRequest(new FrontStatusCode("Hubo un error al guardar el registro de cliente"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format($"{Enums.GetEnumDescription(ApiMessages.DefaultError)} {ex.Message} "));
            }

            return Ok();
        }
    }
}