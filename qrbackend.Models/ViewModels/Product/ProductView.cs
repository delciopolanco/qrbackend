using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Product
{
    public class ProductView
    {
        public string id { get; set; }
        public string entity { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public int type { get; set; }
        public string currency { get; set; }
        public string currencySymbol { get; set; }
        public string balance { get; set; }
        public string subtype { get; set; }
        public string sender { get; set; }
        public string message { get; set; }
        public bool isDefault { get; set; }
    }
}
