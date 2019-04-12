using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Beneficiary
{
    public class DeleteBeneficiary : MQResponse2<DeleteBeneficiary>
    {
        public string BeneficiaryId { get; set; }

        public DeleteBeneficiary()
        {
            FunctionName = "DeleteBeneficiary";
        }
    }
}
