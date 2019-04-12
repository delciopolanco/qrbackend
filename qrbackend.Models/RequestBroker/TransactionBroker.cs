using qrbackend.Models.Entities;
using qrbackend.Models.ViewModels.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.RequestBroker
{
    public class TransactionBroker : JsonResponseGeneric
    {
        public int creditProductType { get; set; } //Del 1 al 5 de acuerdo a tipos de producto de beneficiario
        public bool creditProductMigrate { get; set; }
        public string creditProductNumber { get; set; }
        public string creditProductCurrency { get; set; }
        public string creditProductCurrencySymbol { get; set; }
        public int beneficiaryId { get; set; }
        public int currency { get; set; }
        public int currencySymbol { get; set; }
        public decimal amount { get; set; }
        public decimal tax { get; set; }
        public decimal total { get; set; }
        public decimal foreignExchangeAmount { get; set; }
        public decimal exchangeRate { get; set; }
        public string code { get; set; } //Token de seguridad
        public int debitProductType { get; set; }
        public bool debitProductMigrate { get; set; }
        public string debitProductNumber { get; set; }
        public string debitProductCurrency { get; set; }
        public string debitProductCurrencySymbol { get; set; }
        public string debitProductAlias { get; set; }
        public string debitProductName { get; set; }
        public string debitProductBalance { get; set; }
        public string message { get; set; }
        public bool ongDonation { get; set; }
        public decimal ongAmount { get; set; }
        public string documentIdSender { get; set; }
        public string documentTypeSender { get; set; }

        public string EndPoint { get; set; }
        public string FunctionName { get; set; }

    }
}
