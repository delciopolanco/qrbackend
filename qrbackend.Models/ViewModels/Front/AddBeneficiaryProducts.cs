using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class AddBeneficiaryProducts
    {
        [Required]
        public string Product { get; set; }
        [Required]
        public ProductType ProductType { get; set; }
    }
}
