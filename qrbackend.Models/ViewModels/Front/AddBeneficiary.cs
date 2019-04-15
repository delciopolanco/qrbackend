using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class AddBeneficiary
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string PaymeId { get; set; }

        public List<AddBeneficiaryProducts> Phones { get; set; }

        public List<AddBeneficiaryProducts> Products { get; set; }

    }
}
