using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Token
{
    public class ValidateToken: TransactionTokenBase
    {
        public ValidateToken ()
        {
            FunctionName = "ValidateToken";
        }
    }
}
