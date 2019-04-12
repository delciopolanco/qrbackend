using qrbackend.Models.Entities;
using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.KeyCard
{
    public class KeyCardBase: MQBase
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string KeyCardValue { get; set; }

        [Required]
        public string KeyCardCoordinate { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string DocumentType { get; set; }
    }
}
