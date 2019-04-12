using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Beneficiary
{
    public class GetBeneficiaryList: MQResponse2<List<BeneficiaryBroker>>
    {
        public string DocumentId { get; set; }
        public string DocumentIdClient { get; set; }

        public GetBeneficiaryList()
        {
            FunctionName = "GetBeneficiaryList";
        }
    }
}
