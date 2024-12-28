using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.JunctionTables;
using HippAdministrata.Repositories.Implementation;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Services.Implementation
{

    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public DriverService(IOrderRepository orderRepository, IDriverRepository driverRepository, ApplicationDbContext context, IProductRepository productRepository)
        {
            _driverRepository = driverRepository;
            _context = context;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<Driver> GetByIdAsync(int id)
        {
            return await _driverRepository.GetByIdAsync(id);
        }

        public async Task TransferProductBetweenWarehouses(int productId, int sourceWarehouseId, int destinationWarehouseId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) throw new Exception("Product not found");

            if (product.WarehouseId != sourceWarehouseId)
                throw new Exception("Product is not located in the specified source warehouse");

            // Update the warehouse
            product.WarehouseId = destinationWarehouseId;
            await _productRepository.UpdateProductAsync(product);
        }
        public async Task<IEnumerable<OrderDto>> GetAssignedOrdersAsync(int driverId)
        {
            // Ensure the driver exists
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null) throw new Exception("Driver not found");

            // Fetch orders assigned to the driver
            var orders = await _orderRepository.GetOrdersByDriverIdAsync(driverId);

            // Map orders to DTOs (if needed)
            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                ProductName = order.Product.Name,
                OrderStatus = order.OrderStatus.ToString(),
                Destination = order.DeliveryDestination, // Example field

            });
        }



        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _driverRepository.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(Driver driver)
        {
            return await _driverRepository.UpdateAsync(driver);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _driverRepository.DeleteAsync(id);
        }

       
       
    }
}
