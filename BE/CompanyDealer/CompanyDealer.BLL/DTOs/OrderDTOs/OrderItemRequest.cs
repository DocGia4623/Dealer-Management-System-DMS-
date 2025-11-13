using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.DTOs.OrderDTOs
{
    public class OrderItemRequest
    {
        public Guid VehicleId { get; set; }
        public int Quantity { get; set; }
    }
}
