using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class CompaniesList
    {
        public string DefaultCompany { get; set; }

        public List<Company> Companies { get; set; }
    }
}
