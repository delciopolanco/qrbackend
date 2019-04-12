using qrbackend.Models.Entities;
using qrbackend.Models.ViewModels.Pinqrcode;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Generic
{
    public class JsonResponseGeneric
    {
        public string DescripcionError { get; set; }
        public string CodigoError { get; set; }
        public object Data { get; set; }
    }
}
