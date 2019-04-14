

namespace qrbackend.Models.Products
{
    public class BeneficiaryAccount
    {
        public string DocumentId { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string PhotoImge { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public bool HasDollar { get; set; }

    }
}
