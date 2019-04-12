using qrbackend.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace qrbackend.Models.Entities
{
    public class AuthenticationBase: Entity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public UserType UserType { get; set; }

        [Required]
        public DocumentType DocumentType { get; set; }

        [Required]
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public bool IsLoockedOut { get; set; } = false;
        public DateTime LastLoginDate { get; set; }
    }
}
