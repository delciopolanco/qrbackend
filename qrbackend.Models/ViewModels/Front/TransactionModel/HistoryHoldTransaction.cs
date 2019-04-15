using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front.TransactionModel
{
    public class HistoryHoldTransaction
    {
        public int holdTransactionId { get; set; }
        public string fromAccount { get; set; }
        public string fromPaymeId { get; set; }
        public string toAccount { get; set; }
        public string toPaymeId { get; set; }
        public decimal amount { get; set; }
        public string status { get; set; }
        public string siebelReference { get; set; }
        public string createDateString { get; set; }
        public string holdTransactionType { get; set; }
    }
}
