using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Pinqrcode
{
    public class GetPinqrcode
    {
        
        public string pin { get; set; }
        public string productNumber { get; set; }
        public string productType { get; set; }
        public string productCurrency { get; set; }
        public decimal? amount { get; set; }
        public string beneficiaryName { get; set; }
        public string description { get; set; }
        public string photoImage { get; set; }

    }
}
