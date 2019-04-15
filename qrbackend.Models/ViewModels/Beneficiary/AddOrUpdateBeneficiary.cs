using qrbackend.Models.Entities;
using qrbackend.Models.ViewModels.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Beneficiary
{
    public class AddOrUpdateBeneficiary : JsonResponseGeneric
    {
        public int BeneficiaryId { get; set; }

        public string DocumentId { get; set; }

        public string FullName { get; set; }

        public string BeneficiaryProducts { get; set; }

        public string EndPoint { get; set; }

        public string FunctionName { get; set; }

        public AddOrUpdateBeneficiary(string functionName)
        {
            FunctionName = functionName;
            EndPoint = "beneficiary/addOrUpdate";
        }
    }
}
