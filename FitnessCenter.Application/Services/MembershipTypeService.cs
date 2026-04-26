using FitnessCenter.Application.Interfaces;
using FitnessCenter.Application.Interfaces.Repositories;
using FitnessCenter.Application.Other;
using FitnessCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Services
{
    public class MembershipTypeService : IMembershipTypeService
    {
        private readonly IMembershipTypeRepository _repository;

        public MembershipTypeService(IMembershipTypeRepository repository)
        {
            this._repository = repository;
        }

        public async Task<List<MembershipTypeDto>> GetAllAsync()
        {
            var types = await _repository.GetAllAsync();
            return types.Select(MapToDto).ToList();
        }

        public async Task<List<MembershipTypeDto>> SearchAsync(string? nameFilter, decimal? maxPrice)
        {
            var types = await _repository.SearchAsync(nameFilter, maxPrice);
            return types.Select(MapToDto).ToList();
        }

        public async Task<MembershipTypeDto?> GetByIdAsync(int id)
        {
            var type = await _repository.GetByIdAsync(id);
            return type != null ? MapToDto(type) : null;
        }

        public async Task<int> AddAsync(MembershipTypeDto dto)
        {
            var entity = new MembershipType
            {
                MembershipName = dto.MembershipName,
                PeriodDays = dto.PeriodDays,
                Price = dto.Price,
                Description = dto.Description
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(MembershipTypeDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.TypeId);
            if (entity == null) return false;

            entity.MembershipName = dto.MembershipName;
            entity.PeriodDays = dto.PeriodDays;
            entity.Price = dto.Price;
            entity.Description = dto.Description;

            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static MembershipTypeDto MapToDto(MembershipType type)
        {
            return new MembershipTypeDto
            {
                TypeId = type.Id,
                MembershipName = type.MembershipName,
                PeriodDays = type.PeriodDays,
                Price = type.Price,
                Description = type.Description,
            };
        }
    }
}
