using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front.TransactionModel
{
    public class TransactionQrCodeResp : TransactionQrCode
    {
        public string beneficiaryName { get; set; }
        public string photoImage { get; set; }
        public string ongName { get; set; }
        public decimal tax { get; set; }
        public decimal commision { get; set; }

    }
}
