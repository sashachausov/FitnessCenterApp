using FitnessCenter.Application.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces
{
    public interface IEquipmentService
    {
        Task<List<EquipmentDto>> GetAllAsync();
        Task<List<EquipmentDto>> SearchAsync(EquipmentFilterDto filter);
        Task<EquipmentDto?> GetByIdAsync(int id);
        Task<List<string>> GetEquipmentTypesAsync();
        Task<List<string>> GetManufacturersAsync();
        Task<List<string>> GetStatusesAsync();
        Task<int> AddAsync(EquipmentDto equipment);
        Task<bool> UpdateAsync(EquipmentDto equipment);
        Task<bool> DeleteAsync(int id);        
        Task<List<HallDto>> GetHallsAsync();       
    }
}
