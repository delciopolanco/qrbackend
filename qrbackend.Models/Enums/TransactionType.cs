using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace qrbackend.Models.Enums
{
    public enum TransactionType
    {
        [Description("Débito")]
        Debit = 1,
        [Description("Crédito")]
        Credit = 2
    }
}
