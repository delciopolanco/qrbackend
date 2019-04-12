using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    /// <summary>
    /// Modelo de datos para el inicio de sesion del cliente en Payme
    /// </summary>
    public class Login
    {
        /// <summary>
        /// No de Documento o Identificación del cliente
        /// </summary>
        [Required]
        public string User { get; set; }
        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        [Required]
        public string PassWord { get; set; }
        /// <summary>
        /// Tipo de Identificacion
        /// </summary>
        [Required]
        public DocumentType Type { get; set; }
        /// <summary>
        /// Canal o Tipo de Cliente
        /// </summary>
        [Required]
        public Channel Channel { get; set; }

        /// <summary>
        /// Dispositivo de donde se realiza la peticion.
        /// </summary>
        [Required]
        public string DeviceId { get; set; }

        public Login()
        {
            Type = DocumentType.C;
        }
    }
}
