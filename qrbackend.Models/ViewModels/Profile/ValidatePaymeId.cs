using qrbackend.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace qrbackend.Models.ViewModels.Profile
{
    public class ValidatePaymeId: MQResponse
    {
        [Required]
        public string PaymeId { get; set; }
        [Required]
        public string UserName { get; set; }

        public ValidatePaymeId ()
        {
            FunctionName = "ValidatePaymeId";
        }
    }
}
