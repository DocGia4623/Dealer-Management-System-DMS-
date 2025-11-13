using CompanyDealer.BLL.DTOs.OrderDTOs;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository;
using CompanyDealer.DAL.Repository.VehicleRepo;

namespace CompanyDealer.BLL.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<OrderResponse?> GetByIdAsync(Guid id);
        
        Task<OrderResponse> CreateAsync(CreateOrderRequest request);
        Task<OrderResponse?> UpdateAsync(Guid id, UpdateOrderRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IVehicleRepository _vehicleRepo;

        public OrderService(IOrderRepository orderRepo, IVehicleRepository vehicleRepository)
        {
            _orderRepo = orderRepo;
            _vehicleRepo = vehicleRepository;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return orders.Select(o => new OrderResponse
            {
                Id = o.Id,
                
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CustomerName = o.CustomerName,
                CustomerContact = o.CustomerContact,
                DealerId = o.DealerId,
                DealerName = o.Dealer?.Name
            });
        }

        public async Task<OrderResponse?> GetByIdAsync(Guid id)
        {
            var o = await _orderRepo.GetByIdAsync(id);
            if (o == null) return null;

            return new OrderResponse
            {
                Id = o.Id,
              
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CustomerName = o.CustomerName,
                CustomerContact = o.CustomerContact,
                DealerId = o.DealerId,
                DealerName = o.Dealer?.Name
            };
        }

        public async Task<OrderResponse?> GetByOrderIdAsync(Guid orderId)
        {
            var o = await _orderRepo.GetByIdAsync(orderId);
            if (o == null) return null;

            return new OrderResponse
            {
                Id = o.Id,
               
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CustomerName = o.CustomerName,
                CustomerContact = o.CustomerContact,
                DealerId = o.DealerId,
                DealerName = o.Dealer?.Name
            };
        }

        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
        {

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();
            for (int i = 0; i < request.OrderItems.Length; i++)
            {
                var item = request.OrderItems[i];
                var vehicle = await _vehicleRepo.GetByIdAsync(item.VehicleId);
                if (vehicle == null)
                {
                    throw new Exception($"Vehicle with ID {item.VehicleId} not found");
                }
                var vehiclePrice = vehicle.Price;
                var items = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    VehicleId = item.VehicleId,
                    Quantity = item.Quantity,
                    UnitPrice = vehiclePrice
                };
                orderItems.Add(items);


                totalAmount += vehiclePrice * item.Quantity;
            }
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = request.OrderDate,
                TotalAmount = totalAmount,
                Status = request.Status,
                CustomerName = request.CustomerName,
                CustomerContact = request.CustomerContact,
                DealerId = request.DealerId,
                OrderItems = orderItems
            };

            var created = await _orderRepo.CreateAsync(order);
            return await GetByIdAsync(created.Id) ?? throw new Exception("Failed to retrieve created order");
        }

        public async Task<OrderResponse?> UpdateAsync(Guid id, UpdateOrderRequest request)
        {
            var existing = await _orderRepo.GetByIdAsync(id);
            if (existing == null) return null;
            existing.Status = request.Status;
            existing.CustomerName = request.CustomerName;
            existing.CustomerContact = request.CustomerContact;

            var updated = await _orderRepo.UpdateAsync(existing);
            return updated == null ? null : await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _orderRepo.DeleteAsync(id);
        }
    }
}
