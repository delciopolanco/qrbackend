using qrbackend.Models.Entities;
using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Generic
{
    public class GetGeneric<T>: MQResponse2<T>
    {
        public string EndPoint { get; set; }

        public GetGeneric(GenericEndPoints endPoint, string documentId)
        {
            FunctionName = "GetGeneric";
            EndPoint = string.Concat(Enums.Enums.GetEnumDescription(endPoint), documentId);
        }
    }
}
