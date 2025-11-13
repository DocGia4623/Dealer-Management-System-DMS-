using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.DTOs.OrderDTOs
{
    public class CreateOrderRequest
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;
        public Guid DealerId { get; set; }
        public OrderItemRequest[] OrderItems { get; set; } = Array.Empty<OrderItemRequest>();
    }
}
