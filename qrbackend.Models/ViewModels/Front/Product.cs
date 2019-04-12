using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class Product
    {
        public string Balance { get; set; }

        public string AvailableBalance { get; set; }

        public ProductType2 ProducType { get; set; }

        public bool isDefault { get; set; }

        public string ProductAlias { get; set; }

        public string ProviderName { get; set; }

    }
}
