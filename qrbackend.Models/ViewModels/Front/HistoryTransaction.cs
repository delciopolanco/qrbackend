using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class HistoryTransaction
    {
        public int id { get; set; }
        public string destiny { get; set; }
        public string destinyProduct { get; set; }
        public string type { get; set; }
        public string date { get; set; }
        public string currency { get; set; }
        public string currencySymbol { get; set; }
        public decimal balance { get; set; }
        public string status { get; set; }
    }
}
