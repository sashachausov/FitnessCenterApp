using FitnessCenter.Application.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetAllAsync();
        Task<List<ClientDto>> SearchAsync(string searchText);
        Task<ClientDto?> GetByIdAsync(int id);
        Task<int> AddAsync(ClientDto dto);
        Task<bool> UpdateAsync(ClientDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
