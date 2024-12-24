using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.Enums;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;

namespace HippAdministrata.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public EmployeeService(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task LabelOrderProductAsync(int employeeId, LabelingOrderDto labelingOrderDto)
        {
            // Fetch the order
            var order = await _orderRepository.GetByIdAsync(labelingOrderDto.OrderId);
            if (order == null) throw new Exception("Order not found");

            if (order.EmployeeId != employeeId)
                throw new Exception("You are not assigned to this order");

            // Validate labeling quantity
            if (labelingOrderDto.LabelingQuantity > order.UnlabeledQuantity)
                throw new Exception("Labeling quantity exceeds available unlabeled quantity");

            // Update quantities
            order.LabeledQuantity += labelingOrderDto.LabelingQuantity;
            order.UnlabeledQuantity -= labelingOrderDto.LabelingQuantity;
            order.LastUpdated = DateTime.UtcNow;

            // Check if all quantities are labeled
            if (order.UnlabeledQuantity == 0)
            {
                order.OrderStatus = OrderStatus.ReadyForShipping; // Advance to the next process
            }

            // Save changes
            await _orderRepository.UpdateOrderAsync(order);
        }
    }
}
