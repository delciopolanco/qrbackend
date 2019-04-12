using Newtonsoft.Json;
using qrbackend.Models.Attributes;
using System;
using System.Runtime.Serialization;

namespace qrbackend.Models.Entities
{

    public abstract class MQBase: Entity
    {
        public string FunctionName { get; set; }
    }
}
