using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class Beneficiary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string PaymeId { get; set; }

        public List<BeneficiaryProducts> Phones { get; set; }

        public List<BeneficiaryProducts> Products { get; set; }
    }
}
