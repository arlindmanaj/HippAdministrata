using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
public interface IOrderRequestRepository
{
    Task<OrderRequest> AddAsync(OrderRequest orderRequest);
    Task<OrderRequest?> GetByIdAsync(int requestId);
    Task<List<OrderRequest>> GetPendingRequestsAsync();
    Task<bool> UpdateAsync(OrderRequest orderRequest);
}

}
