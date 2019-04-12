using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.KeyCard
{
    public class ValidateEmpresarialKeyCard: KeyCardBase
    {
        public ValidateEmpresarialKeyCard()
        {
            this.FunctionName = "ValidateEmpresarialKeyCard";
        }
    }
}
