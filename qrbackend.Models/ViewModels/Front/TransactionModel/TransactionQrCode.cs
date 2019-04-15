using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front.TransactionModel
{
    public class TransactionQrCode
    {
        public int creditProductType { get; set; } //Del 1 al 5 de acuerdo a tipos de producto de beneficiario
        public string creditProductNumber { get; set; }
        public string creditProductCurrency { get; set; }
        public string creditProductCurrencySymbol { get; set; }
        public int debitProductType { get; set; } //Del 1 al 5 de acuerdo a tipos de producto de pagador
        public string debitProductNumber { get; set; }
        public string debitProductCurrency { get; set; }
        public string debitProductCurrencySymbol { get; set; }
        public string currency { get; set; }
        public string currencySymbol { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }

    }
}
