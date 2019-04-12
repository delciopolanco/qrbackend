using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Token
{
    public class GenerateTransactionToken: MQResponse
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string DeviceId { get; set; }

        public GenerateTransactionToken()
        {
            FunctionName = "GenerateTransactionToken";
        }
    }
}
