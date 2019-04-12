using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.ViewModels.Front
{
    public class SaveJwtToken
    {
       
        [JsonIgnore]
        public string UserName { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
