using Microsoft.AspNetCore.Http;
using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class FrontClientRegistration
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Contact { get; set; }

        [Required]
        public UserType ClientType { get; set; }

        [Required]
        public string DocumentPicture { get; set; }

        [Required]
        public string Selfie { get; set; }
    }
}
