using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class RegistrationResponse
    {
        public string Name { get; set; }

        public string Document { get; set; }

        public string Address { get; set; }

        public float MeanConfidence { get; set; }
    }
}
