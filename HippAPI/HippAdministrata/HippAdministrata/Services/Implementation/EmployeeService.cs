using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.Enums;
using HippAdministrata.Repositories.Implementation;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;

namespace HippAdministrata.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmployeeRepository _employeeRepository;


        public EmployeeService(IEmployeeRepository employeeRepository ,IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task LabelOrderProductAsync(int employeeId, LabelingOrderDto labelingOrderDto)
        {
            // Fetch the order
            var order = await _orderRepository.GetByIdAsync(labelingOrderDto.OrderId);
            if (order == null) throw new Exception("Order not found");

            if (order.EmployeeId != employeeId)
                throw new Exception("You are not assigned to this order");

            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) throw new Exception("Employee not found");

            var product = await _productRepository.GetByIdAsync(order.ProductId);
            if (product == null) throw new Exception("Product not found");

            // Validate labeling quantity
            if (labelingOrderDto.LabelingQuantity > order.UnlabeledQuantity)
                throw new Exception("Labeling quantity exceeds available unlabeled quantity");

            if (labelingOrderDto.LabelingQuantity > product.UnlabeledQuantity)
                throw new Exception("Insufficient product quantity for labeling");


            decimal paymentPerLabel = product.Price * (product.PricePercentageForEmployee / 100);
            var totalPayment = labelingOrderDto.LabelingQuantity * paymentPerLabel;


            // Update employee's total pay
            employee.TotalPay += totalPayment;

            // Update order quantities
            order.LabeledQuantity += labelingOrderDto.LabelingQuantity;
            order.UnlabeledQuantity -= labelingOrderDto.LabelingQuantity;
            order.LastUpdated = DateTime.UtcNow;

            // Update product quantities
            product.LabeledQuantity += labelingOrderDto.LabelingQuantity;
            product.UnlabeledQuantity -= labelingOrderDto.LabelingQuantity;

            if (product.UnlabeledQuantity < 0)
                throw new Exception("Product unlabeled quantity cannot be negative");

            // Check if all quantities are labeled for the order
            if (order.UnlabeledQuantity == 0)
            {
                order.OrderStatus = OrderStatus.ReadyForShipping; // Advance to the next process
            }

            // Save changes
            await _employeeRepository.UpdateAsync(employee);
            await _orderRepository.UpdateOrderAsync(order);
            await _productRepository.UpdateAsync(product); // Save product changes
        }

    }
    }