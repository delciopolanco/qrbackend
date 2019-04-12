using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Pinqrcode
{
    public class AddPinqrcode
    {
        
        public string productNumber { get; set; }
        public string productType { get; set; }
        public string productCurrency { get; set; }
        public decimal? amount { get; set; }
        public string FunctionName { get; set; }
    }
}
