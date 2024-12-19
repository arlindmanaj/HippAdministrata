﻿using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;

namespace HippAdministrata.Services.Interface
{
    public interface IClientService
    {
        Task<Client> GetByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<bool> UpdateAsync(int id, Client updatedClient);
        Task<bool> DeleteAsync(int id);
        Task<bool> CreateOrderRequestAsync(int clientId, ClientOrderRequestDto request);
        
    }
}
