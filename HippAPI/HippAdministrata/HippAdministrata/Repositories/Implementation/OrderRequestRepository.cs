using HippAdministrata.Data;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Models.Domains;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Repositories.Implementation;


public class OrderRequestRepository : IOrderRequestRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IOrderRepository _orderRepository;
    public OrderRequestRepository(ApplicationDbContext context, IOrderRepository orderRepository)
    {
        _context = context;
        _orderRepository = orderRepository; 
    }

    public async Task<OrderRequest> AddAsync(OrderRequest orderRequest)
    {
        _context.OrderRequests.Add(orderRequest);
        await _context.SaveChangesAsync();
        return orderRequest;
    }

    public async Task<OrderRequest?> GetByIdAsync(int requestId)
    {
        return await _context.OrderRequests.FindAsync(requestId);
    }

    public async Task<List<OrderRequest>> GetPendingRequestsAsync()
    {
        return await _context.OrderRequests.Where(r => r.Status == "Pending").ToListAsync();
    }

    //public async Task<bool> UpdateAsync(OrderRequest orderRequest)
    //{
    //    var existingRequest = await _context.OrderRequests
    //        .FirstOrDefaultAsync(or => or.Id == orderRequest.Id);

    //    if (existingRequest == null)
    //    {
    //        throw new Exception("Order Request not found.");
    //    }

    //    if (existingRequest.Status == "Approved" || existingRequest.Status == "Rejected")
    //    {
    //        throw new Exception("Order Request was already processed.");
    //    }

    //    // Update the request
    //    _context.Entry(existingRequest).CurrentValues.SetValues(orderRequest);
    //    return await _context.SaveChangesAsync() > 0;
    //}
    public async Task<bool> UpdateAsync(OrderRequest orderRequest)
    {
        var existingRequest = await _context.OrderRequests
            .FirstOrDefaultAsync(or => or.Id == orderRequest.Id);

        if (existingRequest == null)
        {
            throw new Exception("Order Request not found.");
        }

        // Remove this check since we are explicitly approving it
        // if (existingRequest.Status == "Approved" || existingRequest.Status == "Rejected")
        // {
        //     throw new Exception("Order Request was already processed.");
        // }

        _context.Entry(existingRequest).CurrentValues.SetValues(orderRequest);
        return await _context.SaveChangesAsync() > 0;
    }





}
