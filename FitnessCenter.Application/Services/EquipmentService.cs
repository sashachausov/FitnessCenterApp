using FitnessCenter.Application.Interfaces;
using FitnessCenter.Application.Interfaces.Repositories;
using FitnessCenter.Application.Other;
using FitnessCenter.Domain.Entities;
using FitnessCenter.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _respository;
        private readonly IHallRepository _hallRepository;

        public EquipmentService(IEquipmentRepository repository, IHallRepository hallRepository) 
        {
            _respository = repository;
            _hallRepository = hallRepository;
        }

        public async Task<List<EquipmentDto>> GetAllAsync()
        {
            var items = await _respository.GetAllWithHallsAsync();
            return items.Select(MapToDto).ToList();
        }

        public async Task<List<EquipmentDto>> SearchAsync(EquipmentFilterDto filter)
        {
            var items = await _respository.SearchAsync(filter);
            return items.Select(MapToDto).ToList();
        }

        public async Task<EquipmentDto?> GetByIdAsync(int id)
        {
            var item = await _respository.GetByIdAsync(id);
            return item != null ? MapToDto(item) : null;
        }

        public Task<List<string>> GetEquipmentTypesAsync()
        {
            var types = Enum.GetValues<EquipmentType>()
                .Select(e => GetEnumDescription(e)).ToList();
            return Task.FromResult(types);
        }

        public Task<List<string>> GetManufacturersAsync()
        {
            var manufacturers = Enum.GetValues<Manufacturer>()
                .Select(m => GetEnumDescription(m)).ToList();
            return Task.FromResult(manufacturers);
        }

        public Task<List<string>> GetStatusesAsync()
        {
            var statuses = Enum.GetValues<EquipmentStatus>()
                .Select(s => GetEnumDescription(s)).ToList();
            return Task.FromResult(statuses);
        }

        public async Task<int> AddAsync(EquipmentDto equipment)
        {
            if (!Enum.TryParse<EquipmentType>(equipment.EquipmentType, out var equipmentType))
                throw new InvalidOperationException(
                    $"Неизвестный тип оборудования: '{equipment.EquipmentType}'. " +
                    "Допустимые значения: " + string.Join(", ", Enum.GetNames<EquipmentType>()));

            if (!Enum.TryParse<Manufacturer>(equipment.Manufacturer, out var manufacturer))
                throw new InvalidOperationException(
                    $"Неизвестный производитель: '{equipment.Manufacturer}'. " +
                    "Допустимые значения: " + string.Join(", ", Enum.GetNames<Manufacturer>()));

            if (!Enum.TryParse<EquipmentStatus>(equipment.EquipmentStatus, out var status))
                throw new InvalidOperationException(
                    $"Неизвестное состояние оборудования: '{equipment.EquipmentStatus}'. " +
                    "Допустимые значения: " + string.Join(", ", Enum.GetNames<EquipmentStatus>()));

            var item = new Equipment
            {
                HallId = equipment.HallId,
                EquipmentName = equipment.EquipmentName,
                InventoryNumber = equipment.InventoryNumber,
                EquipmentType = equipmentType,
                Manufacturer = manufacturer,
                Quantity = equipment.Quantity,
                PurchaseDate = equipment.PurchaseDate,
                EquipmentStatus = status,
                Photo = equipment.PhotoPath ?? "Images/Equipment/ImageByDefault.png"
            };

            await _respository.AddAsync(item);
            return item.Id;
        }

        public async Task<bool> UpdateAsync(EquipmentDto equipment)
        {
            var item = await _respository.GetByIdAsync(equipment.EquipmentId);
            if (item == null) return false;

            if (!Enum.TryParse<EquipmentType>(equipment.EquipmentType, out var equipmentType))
                throw new InvalidOperationException($"Неизвестный тип оборудования: '{equipment.EquipmentType}'.");
            if (!Enum.TryParse<Manufacturer>(equipment.Manufacturer, out var manufacturer))
                throw new InvalidOperationException($"Неизвестный производитель: '{equipment.Manufacturer}'.");
            if (!Enum.TryParse<EquipmentStatus>(equipment.EquipmentStatus, out var status))
                throw new InvalidOperationException($"Неизвестное состояние оборудования: '{equipment.EquipmentStatus}'.");

            item.HallId = equipment.HallId;
            item.EquipmentName = equipment.EquipmentName;
            item.InventoryNumber = equipment.InventoryNumber;
            item.EquipmentType = equipmentType;
            item.Manufacturer = manufacturer;
            item.Quantity = equipment.Quantity;
            item.PurchaseDate = equipment.PurchaseDate;
            item.EquipmentStatus = status;
            item.Photo = equipment.PhotoPath ?? "Images/Equipment/ImageByDefault.png";
        
            await _respository.UpdateAsync(item);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _respository.DeleteAsync(id);
        }

        public async Task<List<HallDto>> GetHallsAsync()
        {
            var halls = await _hallRepository.GetAllAsync();
            return halls.Select(h => new HallDto { HallId = h.Id, HallName = h.HallName }).ToList();
        }

        private static EquipmentDto MapToDto(Equipment equipment)
        {
            return new EquipmentDto
            {
                EquipmentId = equipment.Id,
                HallId = equipment.HallId,
                HallName = equipment.Hall?.HallName ?? "Неизвестный зал",
                EquipmentName = equipment.EquipmentName,
                InventoryNumber = equipment.InventoryNumber,
                EquipmentType = GetEnumDescription(equipment.EquipmentType),
                Manufacturer = GetEnumDescription(equipment.Manufacturer),
                Quantity = equipment.Quantity,
                PurchaseDate = equipment.PurchaseDate,
                EquipmentStatus = GetEnumDescription(equipment.EquipmentStatus),
                PhotoPath = equipment.Photo
            };
        }

        private static string GetEnumDescription<T>(T value) where T : Enum
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                .FirstOrDefault() as System.ComponentModel.DescriptionAttribute;
            return attr?.Description ?? value.ToString();
        }
    }
}
