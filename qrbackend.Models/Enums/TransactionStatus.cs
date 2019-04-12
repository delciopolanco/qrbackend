using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace qrbackend.Models.Enums
{
    public enum TransactionStatus
    {
        [Description("Completado")]
        Complete = 1,
        [Description("Pendiente")]
        Pending = 2
    }
}
