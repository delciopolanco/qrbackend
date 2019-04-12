using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Registration
{
    public class ClientRegistration: MQResponse
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Contact { get; set; }

        [Required]
        public string ClientType { get; set; }

        [Required]
        public string DocumentPicture { get; set; }

        [Required]
        public string Selfie { get; set; }

        public ClientRegistration()
        {
            FunctionName = "ClientRegistration";
        }
    }
}
