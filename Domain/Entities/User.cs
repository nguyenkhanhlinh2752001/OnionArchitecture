using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        [DefaultValue(true)]
        public bool? IsActive { get; set; }
    }
}