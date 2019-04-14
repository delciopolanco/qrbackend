using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class BeneficiaryProducts
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public ProductType ProductType { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public bool WasMigrate { get; set; }
    }
}
