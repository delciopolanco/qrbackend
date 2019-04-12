using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Generic
{
    public class GenericProfile
    {
        public int ClientId { get; set; }
        public string DocumentId { get; set; }
        public string DocumentType { get; set; }  
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PaymeId { get; set; }
        public string IsLoockedOut { get; set; }
        public string ClientTypeId { get; set; }
        public string FavoriteAccount { get; set; }
        public string PhotoImage { get; set; }
    }
}
