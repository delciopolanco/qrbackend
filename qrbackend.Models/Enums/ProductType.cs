using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace qrbackend.Models.Enums
{
    public enum ProductType
    {
        [Description("PaymeId")]
        PaymeId = 1,
        [Description("Ahorro")]
        SaveAccount = 2,
        [Description("Corriente")]
        CheckingAccount = 3,
        [Description("Tarjeta")]
        CreditCard = 4,
        [Description("Telefono")]
        Phone = 5
    }

    public enum ProductType2
    {
        [Description("PaymeId")]
        PaymeId = 1,
        [Description("Cuentas de Ahorro")]
        SaveAccount = 2,
        [Description("Cuentas Corrientes")]
        CheckingAccount = 3,
        [Description("Tarjetas de Credito")]
        CreditCard = 4,
        [Description("GC")]
        GiftCard = 6
    }

    public enum ProductTypeBroker
    {
        [Description("CC")]
        CC = 1,
        [Description("CA")]
        CA = 2,
        [Description("TC")]
        TC = 3
    }

}
