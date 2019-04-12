using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front.Client
{
    public class SavingPlan
    {
        public string toAccount { get; set; }
        public string fromAccount { get; set; }
        public decimal amount { get; set; }
    }
}
