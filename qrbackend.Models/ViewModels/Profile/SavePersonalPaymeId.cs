using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Profile
{
    public class SavePersonalPaymeId: MQResponse
    {
        public string PaymeId { get; set; }
        public string UserName { get; set; }

        public SavePersonalPaymeId ()
        {
            FunctionName = "SavePaymeId";
        }
    }
}
