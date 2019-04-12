using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class TransactionQrCodeBroker : TransactionQrCode
    {
        public string EndPoint { get; set; }
        public string FunctionName { get; set; }
        
    }
}
