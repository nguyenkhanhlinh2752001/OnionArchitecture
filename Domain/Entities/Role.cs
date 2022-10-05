using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Role: BaseEntity
    {
        public string Name { get; set; }
    }
}