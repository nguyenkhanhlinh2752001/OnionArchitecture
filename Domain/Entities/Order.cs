using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order: BaseEntity
    {
        public int CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
