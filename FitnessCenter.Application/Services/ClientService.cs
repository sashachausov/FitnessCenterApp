using FitnessCenter.Application.Interfaces;
using FitnessCenter.Application.Interfaces.Repositories;
using FitnessCenter.Application.Other;
using FitnessCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ClientDto>> GetAllAsync()
        {
            var clients = await _repository.GetAllAsync();
            return clients.Select(MapToDto).ToList();
        }

        public async Task<List<ClientDto>> SearchAsync(string searchText)
        {
            var clients = await _repository.SearchAsync(searchText);
            return clients.Select(MapToDto).ToList();
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            var client = await _repository.GetByIdAsync(id);
            return client != null ? MapToDto(client) : null;
        }

        public async Task<int> AddAsync(ClientDto dto)
        {
            var client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                BirthDate = dto.BirthDate,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _repository.AddAsync(client);   // уже сохраняет внутри
            return client.Id;
        }

        public async Task<bool> UpdateAsync(ClientDto dto)
        {
            var client = await _repository.GetByIdAsync(dto.Id);
            if (client == null) return false;

            client.FirstName = dto.FirstName;
            client.LastName = dto.LastName;
            client.MiddleName = dto.MiddleName;
            client.BirthDate = dto.BirthDate;
            client.PhoneNumber = dto.PhoneNumber;
            client.Email = dto.Email;

            await _repository.UpdateAsync(client);   // уже сохраняет внутри
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Невозможно удалить клиента: в базе данных обнаружены связанные записи.");
            }
        }

        private static ClientDto MapToDto(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                MiddleName = client.MiddleName,
                BirthDate = client.BirthDate,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
                RegistrationDate = client.RegistrationDate
            };
        }
    }
}
