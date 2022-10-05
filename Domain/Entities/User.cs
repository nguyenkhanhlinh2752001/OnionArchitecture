using Domain.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
        public string ResetToken { get; set; }

        public DateTime ResetTokenExpiryDate { get; set; }
    }
}