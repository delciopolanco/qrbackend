using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Token
{
    public abstract class TransactionTokenBase: MQResponse
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string TransactionToken { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        public string DeviceId { get; set; }
    }
}
