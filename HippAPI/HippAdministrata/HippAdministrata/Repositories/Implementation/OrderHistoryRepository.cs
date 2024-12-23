using HippAdministrata.Data;
using HippAdministrata.Models.JunctionTables;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace HippAdministrata.Repositories.Implementation
{
    public class OrderHistoryRepository : IOrderHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        //public Task AddAsync(OrderHistory historyEntry)
        //{
        //    if (historyEntry == null)
        //        throw new ArgumentNullException(nameof(historyEntry));

        //    // Add the history entry to the DbContext
        //    _context.OrderHistories.Add(historyEntry);

        //    // Save changes asynchronously
        //    await _context.SaveChangesAsync();
        //}
    }
}
