using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModels
{
    public class GetAllUsersVM
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
