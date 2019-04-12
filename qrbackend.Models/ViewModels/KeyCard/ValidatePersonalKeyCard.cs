using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.KeyCard
{
    public class ValidatePersonalKeyCard: KeyCardBase
    {
        public ValidatePersonalKeyCard()
        {
            FunctionName = "ValidatePersonalKeyCard";
        }
    }
}
