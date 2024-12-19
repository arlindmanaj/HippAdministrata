using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.JunctionTables;

namespace HippAdministrata.Services.Interface
{
    public interface ISalesPersonClientService
    {
        Task<IEnumerable<SalesPersonClientsDto>> GetAllPendingAsync();
        Task<bool> ProcessOrderRequestAsync(int requestId, OrderProcessRequestDto request);
    }
}
