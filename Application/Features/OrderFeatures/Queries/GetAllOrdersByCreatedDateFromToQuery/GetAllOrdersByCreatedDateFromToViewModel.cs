using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderFeatures.Queries.GetAllOrdersByCreatedDateFromToQuery
{
    public class GetAllOrdersByCreatedDateFromToViewModel
    {
        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
