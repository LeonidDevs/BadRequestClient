using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VittaClient.Models
{
    public class OrderItemDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual ProductDTO Product { get; set; } = null!;

    }
}
