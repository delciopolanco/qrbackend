using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Token
{
    public class JwtToken: MQResponse
    {
        public string isFirstLogin { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
