using qrbackend.Models.Entities;
using qrbackend.Models.ViewModels.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.RequestBroker
{
    public class SaveImageBroker : JsonResponseGeneric
    {
        public string documentId { get; set; }
        public string documentType { get; set; }
        public string photoImageB64 { get; set; }
        public string FunctionName { get; set; }
        public string EndPoint { get; set; }
    }
}
