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
using System.Transactions;

namespace FitnessCenter.Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _repository;
        private readonly IMembershipTypeRepository _typeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public MembershipService(IMembershipRepository repository, IMembershipTypeRepository typeRepository, IClientRepository clientRepository, IDateTimeProvider dateTimeProvider)
        {
            this._repository = repository;
            this._typeRepository = typeRepository;
            this._clientRepository = clientRepository;
            this._dateTimeProvider = dateTimeProvider;
        }

        public async Task<IEnumerable<MembershipDto>> GetAllActiveAndFrozenMembershipsAsync()
        {
            var memberships = await _repository.GetAllActiveAndFrozenWithClientsAsync();
            return memberships.Select(MapToDto).ToList();
        }

        public async Task<IEnumerable<MembershipDto>> GetActiveAndFrozenByClientAsync(int clientId)
        {
            var memberships = await _repository.GetActiveAndFrozenByClientIdAsync(clientId);
            return memberships.Select(MapToDto).ToList();
        }

        public async Task<IEnumerable<MembershipDto>> GetMembershipHistoryAsync(int clientId)
        {
            var memberships = await _repository.GetAllByClientIdAsync(clientId);
            return memberships.Select(MapToDto).ToList();
        }

        public async Task<MembershipDto?> GetByIdAsync(int membershipId)
        {
            var membership = await _repository.GetByIdAsync(membershipId);
            return membership != null ? MapToDto(membership) : null;
        }

        public async Task<MembershipDto> SellToNewClientAsync(string fullName, string phoneNumber, int typeId)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var membershipType = await _typeRepository.GetByIdAsync(typeId)
                ?? throw new InvalidOperationException("Тип абонемента не найден.");

            var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var newClient = new Client
            {
                LastName = nameParts.ElementAtOrDefault(0) ?? fullName,
                FirstName = nameParts.ElementAtOrDefault(1) ?? "",
                MiddleName = nameParts.ElementAtOrDefault(2),
                PhoneNumber = phoneNumber,
                RegistrationDate = _dateTimeProvider.Today,
            };

            await _clientRepository.AddAsync(newClient);

            decimal actualCost = membershipType.Price * 0.85m;
            var startDate = _dateTimeProvider.Today;
            DateOnly? endDate = membershipType.PeriodDays.HasValue ? startDate.AddDays(membershipType.PeriodDays.Value) : null;

            var membership = new Membership
            {
                ClientId = newClient.Id,
                TypeId = typeId,
                StartDate = startDate,
                EndDate = endDate,
                ActualCost = actualCost,
                SellDate = startDate,
                Status = MembershipStatus.Active
            };

            await _repository.AddAsync(membership);

            // Получаем DTO внутри транзакции (безопасно, так как данные уже сохранены)
            var result = await GetByIdAsync(membership.Id);
            scope.Complete();

            return result ?? throw new InvalidOperationException("Не удалось создать абонемент");
        }

        public async Task<MembershipDto> SellToExistingClientAsync(int clientId, int typeId)
        {
            var existingActive = await _repository.GetActiveAndFrozenByClientIdAsync(clientId);
            if (existingActive.Any(m => m.Status == MembershipStatus.Active))
                throw new InvalidOperationException("У клиента уже есть действующий абонемент.");

            var membershipType = await _typeRepository.GetByIdAsync(typeId) ?? 
                throw new InvalidOperationException("Тип абонемента не найден.");

            var startDate = _dateTimeProvider.Today;
            DateOnly? endDate = membershipType.PeriodDays.HasValue ? startDate.AddDays(membershipType.PeriodDays.Value) : null;

            var membership = new Membership
            {
                ClientId = clientId,
                TypeId = typeId,
                StartDate = startDate,
                EndDate = endDate,
                ActualCost = membershipType.Price,
                SellDate = startDate,
                Status = MembershipStatus.Active
            };

            await _repository.AddAsync(membership);
            await _repository.SaveChangesAsync();

            return await GetByIdAsync(membership.Id) ?? throw new InvalidOperationException("Не удалось создать абонемент");
        }

        public async Task<bool> ExtendMembershipAsync(int membershipId)
        {
            var membership = await _repository.GetByIdAsync(membershipId);
            if (membership == null || membership.Status != MembershipStatus.Frozen)
                return false;

            var type = await _typeRepository.GetByIdAsync(membership.TypeId);
            if (type?.PeriodDays == null) return false;

            var today = _dateTimeProvider.Today;
            int usedDays = membership.FreezeDaysUsed ?? 0;
            int remainingDays = type.PeriodDays.Value - usedDays;

            membership.EndDate = today.AddDays(remainingDays);
            membership.Status = MembershipStatus.Active;
            membership.FrozenDate = null;
            membership.FreezeDaysUsed = null;
            membership.HasBeenFrozen = true;
            membership.UpdatedAt = _dateTimeProvider.UtcNow;

            await _repository.UpdateAsync(membership);          
            return true;
        }

        public async Task<bool> SuspendMembershipAsync(int membershipId)
        {
            var membership = await _repository.GetByIdAsync(membershipId);
            if (membership == null || membership.Status != MembershipStatus.Active)
                return false;

            if (membership.HasBeenFrozen) throw new InvalidOperationException("Абонемент уже был ранее заморожен. Повторная заморозка запрещена.");

            var today = _dateTimeProvider.Today;
            membership.FreezeDaysUsed = today.DayNumber - membership.StartDate.DayNumber;
            membership.FrozenDate = today;
            membership.Status = MembershipStatus.Frozen;
            membership.UpdatedAt = _dateTimeProvider.UtcNow;

            await _repository.UpdateAsync(membership);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteMembershipAsync(int membershipId)
        {
            var membership = await _repository.GetByIdAsync(membershipId);
            if (membership == null) return false;

            var today = _dateTimeProvider.Today;
            if (membership.Status != MembershipStatus.Active || membership.EndDate == null || membership.EndDate.Value > today)
                return false;

            membership.Status = MembershipStatus.Completed;
            membership.UpdatedAt = _dateTimeProvider.UtcNow;

            await _repository.UpdateAsync(membership);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelMembershipAsync(int membershipId)
        {
            var membership = await _repository.GetByIdAsync(membershipId);
            if (membership == null) return false;

            membership.Status = MembershipStatus.Canceled;
            membership.UpdatedAt = _dateTimeProvider.UtcNow;

            await _repository.UpdateAsync(membership);
            await _repository.SaveChangesAsync();
            return true;
        }

        private static MembershipDto MapToDto(Membership membership)
        {
            return new MembershipDto
            {
                MembershipId = membership.Id,
                ClientId = membership.ClientId,
                ClientFullName = membership.Client != null ? $"{membership.Client.LastName} {membership.Client.FirstName} {membership.Client.MiddleName}".Trim() : string.Empty,
                TypeId = membership.TypeId,
                MembershipName = membership.Type?.MembershipName ?? "Неизвестный тип",
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                ActualCost = membership.ActualCost,
                SellDate = membership.SellDate,
                MembershipStatus = membership.Status.ToString(),
                StatusDisplayName = GetEnumDescription(membership.Status)
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
