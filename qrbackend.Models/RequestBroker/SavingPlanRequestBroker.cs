using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.RequestBroker
{
    public class SavingPlanRequestBroker
    {
        public string toAccount { get; set; }
        public string fromAccount { get; set; }
        public decimal amount { get; set; }
        public string documentId { get; set; }
        public string documentType { get; set; }
        public string FunctionName { get; set; }
        public string EndPoint { get; set; }
    }
}
