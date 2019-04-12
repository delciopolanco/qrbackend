using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Beneficiary
{
    public class AddOrUpdateBeneficiary : MQResponse3
    {
        public string BeneficiaryId { get; set; }

        public string DocumentId { get; set; }

        public string FullName { get; set; }

        public string BeneficiaryProducts { get; set; }

        public AddOrUpdateBeneficiary()
        {
            FunctionName = "AddOrUpdateBeneficiary";
        }
    }
}
