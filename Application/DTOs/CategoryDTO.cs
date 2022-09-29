using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CategoryDTO
    {
        public string CategoryName { get; set; }
        public IEnumerable<ProductDTO> Products { get; set; }
    }
}
