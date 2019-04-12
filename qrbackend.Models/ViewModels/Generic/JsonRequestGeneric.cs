using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Generic
{
    [Serializable]
   public class JsonRequestGeneric : MQResponse4<JsonResponseGeneric>
    {
        
        public string FunctionName { get; set; }

        public string EndPoint { get; set; }

        public Object Data { get; set; }

        public JsonRequestGeneric(string functionName) {
            FunctionName = functionName;
        }

    }
}
