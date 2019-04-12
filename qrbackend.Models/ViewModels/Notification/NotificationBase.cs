using qrbackend.Models.Entities;
using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Notification
{
    public abstract class NotificationBase : MQResponse
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string NotificationType { get; set; }

        //[Required]
        //public string NotificationConfig { get; set; } = string.Empty;

        [Required]
        public string DeviceId { get; set; }


        private string config = string.Empty;

        [Required]
        public string NotificationConfig
        {
            get
            {
                if (NotificationType == ((int)UserNotificationType.Email).ToString())
                {
                    config = string.Empty;
                    return config;
                }
                return config;
            }
            set
            {
                config = value;
            }
        }
    }
}
