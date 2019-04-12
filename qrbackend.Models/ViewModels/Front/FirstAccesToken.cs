using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class FirstAccesToken
    {
        public bool FirstTimeAccess { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
