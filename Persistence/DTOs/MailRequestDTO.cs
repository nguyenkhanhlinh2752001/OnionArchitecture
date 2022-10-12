using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Persistence.DTOs
{
    public class MailRequestDTO
    {
        [EmailAddress]
        public string ToEmail { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}