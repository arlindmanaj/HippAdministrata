using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;

namespace HippAdministrata.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDriverRepository _driverRepository;

        public OrderService(IOrderRepository orderRepository, IEmployeeRepository employeeRepository, IDriverRepository driverRepository)
        {
            _orderRepository = orderRepository;
            _employeeRepository = employeeRepository;
            _driverRepository = driverRepository;
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
        public async Task<bool> AssignEmployeeToOrder(int orderId, int employeeId)
        {
            // Validate if the order exists
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException($"Order with ID {orderId} not found.");

            // Validate if the employee exists
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new ArgumentException($"Employee with ID {employeeId} not found.");

            // Assign the employee to the order
            order.EmployeeId = employeeId;
            order.Employee = employee; // Optional: Attach the employee object if needed

            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> AssignDriverToOrder(int orderId, int driverId)
        {
            // Validate if the order exists
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException($"Order with ID {orderId} not found.");

            // Validate if the driver exists
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
                throw new ArgumentException($"Driver with ID {driverId} not found.");

            // Assign the driver to the order
            order.DriverId = driverId;
            order.Driver = driver; // Optional: Attach the driver object if needed

            return await _orderRepository.UpdateAsync(order);
        }
    }
}
