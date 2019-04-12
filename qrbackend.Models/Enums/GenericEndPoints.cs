using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace qrbackend.Models.Enums
{
    public enum GenericEndPoints
    {
        [Description("client/get?DocumentId=")]
        ClientGet,
        [Description("beneficiary/validateBeneficiaryByAccount?Account=")]
        BeneficiaryValidateAccount,
        [Description("beneficiary/getByPaymeId?PaymeId=")]
        GetByPaymeId
    }
}
