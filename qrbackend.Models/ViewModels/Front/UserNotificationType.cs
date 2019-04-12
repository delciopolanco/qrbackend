using qrbackend.Models.Attributes;
using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class SaveNotificationType
    {
        [Required]
        public UserNotificationType Type { get; set; }

       // [RequiredIf("Type", "sms")]
        public string TargetDevice { get; set; }

        [Required]
        public string KeyCardValue { get; set; }

        [Required]
        public string DeviceId { get; set; }
    }
}
