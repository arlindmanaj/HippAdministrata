using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;

namespace HippAdministrata.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
        {
            return await _orderRepository.GetByClientIdAsync(clientId);
        }

        public async Task<IEnumerable<Order>> GetBySalesPersonIdAsync(int salesPersonId)
        {
            return await _orderRepository.GetBySalesPersonIdAsync(salesPersonId);
        }

        public async Task<bool> CreateAsync(Order order)
        {
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<bool> UpdateAsync(Order order)
        {
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;

            order.OrderStatus = status;
            order.LastUpdated = DateTime.UtcNow;

            return await _orderRepository.UpdateAsync(order);
        }
    }
}
