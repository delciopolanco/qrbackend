using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace qrbackend.Models.Enums
{
    public enum ResponseCode
    {
        [Description("00")]
        Success,

        [Description("01")]
        Fail,

        [Description("02")]
        Expired
    }
}
