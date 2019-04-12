
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class Profile
    {
        public string Name { get; set; }
        public string PayMeID { get; set; }
        public string Email { get; set; }
        public bool ExistInAlternateDomain { get; set; }
        public string Image { get; set; }
    }
}
