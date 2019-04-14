using qrbackend.Models.Enums;
using System.Collections.Generic;

namespace qrbackend.Models.ViewModels.Front
{
    public class ClientValidationProduct
    {
        public string ClientName { get; set; }
        public ProductType2 ProductType { get; set; }
        //public List<Currency> Currencies { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public bool HasDollar { get; set; }
    }
}
