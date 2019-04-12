using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class SavePaymeId
    {
        [Required]
        public string code { get; set; }

        [Required]
        public string PayMeID { get; set; }

        [Required]
        public string DeviceId { get; set; }
    }
}
