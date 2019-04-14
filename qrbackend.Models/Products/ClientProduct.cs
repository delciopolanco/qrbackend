using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.Products
{
    public class ClientProduct
    {
        public string ProductNumber { get; set; }
        public string Alias { get; set; }
        public string CurrentBalance { get; set; }
        public string CurrentBalanceUS { get; set; }
        public string AvailableBalance { get; set; }
        public string AvailableBalanceUS { get; set; }
        public string ProducType { get; set; }
        public string AccountStatus { get; set; }
        public string StatusCode { get; set; }
        public string Amount { get; set; }
        public string Message { get; set; }
        public string ProviderName { get; set; }
        public bool IsFavorite { get; set; }
        public bool HasDollar { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public string Name { get; set; }
        public string Subtype { get; set; }
        public bool WasMigrate { get; set; }


    }
}
