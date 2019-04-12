using Newtonsoft.Json;
using System;


namespace qrbackend.Models.Entities
{
    public abstract class Entity
    {
        [JsonIgnore]
        private int ID { get; set; }
        [JsonIgnore]
        private DateTime CreationDate { get; set; }
    }
}
