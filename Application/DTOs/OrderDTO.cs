using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class OrderDTO: BaseDTO
    {
        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
