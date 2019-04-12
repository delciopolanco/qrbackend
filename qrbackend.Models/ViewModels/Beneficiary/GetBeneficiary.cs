using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Beneficiary
{
    public class GetBeneficiary: MQResponse2<BeneficiaryBroker>
    {
        public string BeneficiaryId { get; set; }

        public string DocumentId { get; set; }

        public GetBeneficiary()
        {
            FunctionName = "GetBeneficiary";
        }
    }
}
