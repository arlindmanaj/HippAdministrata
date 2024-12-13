using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;

namespace HippAdministrata.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, string status)
        {
            return await _orderRepository.UpdateOrderStatusAsync(id, status);
        }
    }
}
