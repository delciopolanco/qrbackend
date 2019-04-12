using Newtonsoft.Json;
using qrbackend.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace qrbackend.Models.ViewModels.Login
{
    public abstract class LoginModelBase: MQResponse
    {
        /// <summary>
        /// No de Documento o Identificación del cliente
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Contraseña utilizada para ingresar
        /// </summary>
        [Required]
        public string PassWord { get; set; }

        /// <summary>
        /// Contraseña utilizada para ingresar
        /// </summary>
        [Required]
        public bool isFirstLogin { get; set; }

        /// <summary>
        /// Dispositivo desde donde se realiza la peticion.
        /// </summary>
        [Required]
        public string DeviceId { get; set; }


        public string DocumentType { get; set; }
    }
}
