using Newtonsoft.Json;
using qrbackend.Models.Entities;
using qrbackend.Models.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels
{
    public class HelloWorld: LoginModelBase
    {
        [JsonIgnore]
        public string Hola { get; set; }
        
    }
}
