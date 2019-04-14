using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Beneficiary
{
    public class BeneficiaryProduct
    {
        public int beneficiaryProductId { get; set; }
        public int beneficiaryId { get; set; }
        public string value { get; set; }
        public int beneficiaryProductTypeId { get; set; }
        public string currency { get; set; }
        public string currencySymbol { get; set; }
        public bool hasDollar { get; set; }
        public bool wasMigrate { get; set; }


    }
}
