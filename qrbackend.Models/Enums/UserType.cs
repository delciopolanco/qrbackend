using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace qrbackend.Models.Enums
{
    public enum UserType
    {
        [Description("Personal")]
        Personal = 1,

        [Description("Empresarial")]
        Empresarial = 2
    }
}
