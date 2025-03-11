using System.Xml.Serialization;
using HippAdministrata.Models.Domains;


namespace HippAdministrata.Services.Interface

{
    public interface IOrderRequestService
    {
        Task<OrderRequest> CreateOrderRequestAsync(int orderId, int clientId, string requestType, string? reason);
        Task<List<OrderRequest>> GetPendingRequestsAsync();
        Task<bool> ApproveRequestAsync(int requestId);
        Task<bool> RejectRequestAsync(int requestId, string reason);
    }

}
