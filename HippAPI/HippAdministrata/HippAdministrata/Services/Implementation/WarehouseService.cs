using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;

namespace HippAdministrata.Services
{
    public class WarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Warehouse> GetByIdAsync(int id) => await _warehouseRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Warehouse>> GetAllAsync() => await _warehouseRepository.GetAllAsync();

        public async Task<IEnumerable<Order>> GetOrdersByWarehouseAsync(int warehouseId)
        {
            return await _warehouseRepository.GetOrdersByWarehouseAsync(warehouseId);
        }

        public async Task<bool> CreateAsync(Warehouse warehouse) => await _warehouseRepository.CreateAsync(warehouse);

        public async Task<bool> UpdateAsync(Warehouse warehouse) => await _warehouseRepository.UpdateAsync(warehouse);

        public async Task<bool> DeleteAsync(int id) => await _warehouseRepository.DeleteAsync(id);
    }
}
