using Newtonsoft.Json;
using qrbackend.Models.Attributes;
using System;
using System.Runtime.Serialization;

namespace qrbackend.Models.Entities
{

    public abstract class MQResponse: MQBase
    {
        public string Code { get; set; }

        public string Messages { get; set; }
    }

    public abstract class MQResponse2<T> : MQBase
    {
        public string CodigoError { get; set; }

        public string DescripcionError { get; set; }

        public T Data { get; set; }
    }

    public abstract class MQResponse3 : MQBase
    {
        public string CodigoError { get; set; }

        public string DescripcionError { get; set; }
    }

    public class MQResponse4<T> : MQBase
    {
        public string CodigoError { get; set; }

        public string DescripcionError { get; set; }

        public T Data { get; set; }
    }

    public class MQResponseResult
    {
        public string Code { get; set; }

        public string Messages { get; set; }
    }
}
