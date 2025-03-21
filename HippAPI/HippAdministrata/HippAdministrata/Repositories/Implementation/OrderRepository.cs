﻿using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;


        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Product) // Include navigation properties if needed
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetByIdWithProductAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Set<Order>().Include(x => x.OrderStatus).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
        {
            return await _context.Set<Order>().Where(o => o.ClientId == clientId).Include(x => x.OrderStatus).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetBySalesPersonIdAsync(int salesPersonId)
        {
            return await _context.Set<Order>().Where(o => o.SalesPersonId == salesPersonId).ToListAsync();
        }

        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        //public async Task<bool> UpdateAsync(Order order)
        //{
        //    _context.Set<Order>().Update(order);
        //    return await _context.SaveChangesAsync() > 0;
        //}
        // OrderRepository.cs

        public async Task<bool> UpdateAsync(Order order)
        {
            _context.Set<Order>().Update(order);
            return await _context.SaveChangesAsync() > 0; // Return true if any rows were affected, else false
        }


        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                _context.Set<Order>().Remove(order);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<List<OrderDto>> GetOrdersBySalesPersonIdAsync(int salesPersonId)
        {
            return await _context.Orders
                .Where(o => o.SalesPersonId == salesPersonId).Include(x => x.OrderStatus)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    ProductName = o.Product.Name, // Assuming navigation property exists
                    ClientName = o.Client.Name,  // Assuming navigation property exists
                    SalesPersonName = o.SalesPerson.Name, // Assuming navigation property exists
                    DeliveryDestination = o.DeliveryDestination,
                    Quantity = o.Quantity,
                    UnlabeledQuantity = o.UnlabeledQuantity,
                    LabeledQuantity = o.LabeledQuantity,
                    ProductPrice = o.ProductPrice,
                    CreatedAt = o.CreatedAt,
                    OrderStatusId = o.OrderStatusId,
                    OrderStatusDescription = o.OrderStatus.Description
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, string status)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                order.LastUpdated = DateTime.UtcNow;
                // Update the status field here (depends on your model structure).
                return await UpdateAsync(order);
            }
            return false;
        }

        public async Task<IEnumerable<Order>> GetOrdersByEmployeeIdAsync(int employeeId)
        {
            return await _context.Orders
                .Where(order => order.EmployeeId == employeeId)
                .Include(order => order.Product)
                .Include(order => order.OrderStatus)// Include related product details
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByDriverIdAsync(int driverId)
        {
            return await _context.Orders
                .Where(order => order.DriverId == driverId)
                .Include(order => order.Product).Include(x => x.OrderStatus) // Include product details
                .ToListAsync();
        }


    }
}
