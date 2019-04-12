using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Beneficiary
{
    public class ClientsBeneficiary: MQResponse
    {
        public int beneficiaryId { get; set; }
        public string documentId { get; set; }
        public string name { get; set; }
        public string nickName { get; set; }
        public string photoImage { get; set; }

        public List<BeneficiaryProduct> BeneficiaryProduct { get; set; }
    }
}
