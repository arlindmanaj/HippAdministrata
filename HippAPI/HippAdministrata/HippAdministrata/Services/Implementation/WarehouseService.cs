using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;

namespace HippAdministrata.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _repository;

        public WarehouseService(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Warehouse> CreateWarehouseAsync(string name, Location location)
        {
            var warehouse = new Warehouse
            {
                Name = name,
                Location = location
            };
            return await _repository.CreateAsync(warehouse);
        }

        public async Task<Warehouse?> GetWarehouseByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Warehouse>> GetAllWarehousesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Warehouse> UpdateWarehouseAsync(int id, string name, Location location)
        {
            var warehouse = await _repository.GetByIdAsync(id);
            if (warehouse == null)
                throw new Exception($"Warehouse with ID {id} not found.");

            warehouse.Name = name;
            warehouse.Location = location;

            return await _repository.UpdateAsync(warehouse);
        }

        public async Task DeleteWarehouseAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }

}
