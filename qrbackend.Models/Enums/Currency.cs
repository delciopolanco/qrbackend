using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace qrbackend.Models.Enums
{
    public enum Currency
    {
        [Description("Pesos")]
        Pesos,
        [Description("Dolar")]
        Dolar,
        [Description("Euro")]
        Euro
    }
}
