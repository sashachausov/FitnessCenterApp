using FitnessCenter.Application.Interfaces;
using FitnessCenter.Application.Interfaces.Repositories;
using FitnessCenter.Application.Other;
using FitnessCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FitnessCenter.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;

        public EmployeeService(IEmployeeRepository employeeRepository, IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher passwordHasher) 
        {
            this._employeeRepository = employeeRepository;
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._passwordHasher = passwordHasher;
        }

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.Select(MapToDto).ToList();
        }

        public async Task<List<EmployeeDto>> SearchAsync(EmployeeFilterDto filter)
        {
            var employees = await _employeeRepository.SearchAsync(filter);
            return employees.Select(MapToDto).ToList();
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var employees = await _employeeRepository.GetByIdWithUserAsync(id);
            return employees != null ? MapToDto(employees) : null;
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => new RoleDto { RoleId = r.Id, RoleName = r.RoleName }).ToList();
        }

        public async Task<int> AddAsync(EmployeeDto entity, string? userName, string? password)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            User? user = null;
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                var existingUser = await _userRepository.GetByUserNameAsync(userName);
                if (existingUser != null)
                    throw new InvalidOperationException("Пользователь с таким логином уже существует.");

                user = new User
                {
                    UserName = userName,
                    PasswordHash = _passwordHasher.HashPassword(password),
                    RoleId = entity.RoleId ?? throw new InvalidOperationException("Не указана роль пользователя.")
                };
                await _userRepository.AddAsync(user);
            }

            var employee = new Employee
            {
                UserId = user?.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                BirthDate = entity.BirthDate,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                HireDate = entity.HireDate ?? DateOnly.FromDateTime(DateTime.Today),
                IsActive = entity.IsActive,
                Photo = entity.PhotoPath ?? "Images/Employees/ImageByDefault.png"
            };

            await _employeeRepository.AddAsync(employee);
            scope.Complete();
            return employee.Id;
        }

        public async Task<bool> UpdateAsync(EmployeeDto entity, string? userName, string? password, bool? resetPassword)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Загружаем сотрудника вместе с пользователем
            var employee = await _employeeRepository.GetByIdWithUserAsync(entity.EmployeeId);
            if (employee == null) return false;

            // Обновляем данные сотрудника
            employee.FirstName = entity.FirstName;
            employee.LastName = entity.LastName;
            employee.MiddleName = entity.MiddleName;
            employee.BirthDate = entity.BirthDate;
            employee.PhoneNumber = entity.PhoneNumber;
            employee.Email = entity.Email;
            employee.HireDate = entity.HireDate ?? DateOnly.FromDateTime(DateTime.Today);
            employee.IsActive = entity.IsActive;
            employee.Photo = entity.PhotoPath ?? "Images/Employees/ImageByDefault.png";

            // Работа с пользователем (User)
            if (employee.User == null)
            {
                // Создаём нового пользователя, если его ещё нет
                if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
                {
                    var existingUser = await _userRepository.GetByUserNameAsync(userName);
                    if (existingUser != null)
                        throw new InvalidOperationException("Пользователь с таким логином уже существует.");

                    var newUser = new User
                    {
                        UserName = userName,
                        PasswordHash = _passwordHasher.HashPassword(password),
                        RoleId = entity.RoleId ?? throw new InvalidOperationException("Не указана роль пользователя.")
                    };

                    await _userRepository.AddAsync(newUser);                    
                    employee.UserId = newUser.Id;
                }
            }
            else
            {
                // Обновляем существующего пользователя
                if (!string.IsNullOrWhiteSpace(userName))
                    employee.User.UserName = userName;

                if (resetPassword == true && !string.IsNullOrWhiteSpace(password))
                    employee.User.PasswordHash = _passwordHasher.HashPassword(password);

                if (entity.RoleId.HasValue)
                    employee.User.RoleId = entity.RoleId.Value;

                // Важно: говорим EF Core, что сущность изменена
                await _userRepository.UpdateAsync(employee.User);
            }

            // Сохраняем изменения сотрудника
            await _employeeRepository.UpdateAsync(employee);
            scope.Complete();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdWithUserAsync(id);
            if (employee == null) return false;
            
            if (employee.User != null)
                await _userRepository.DeleteAsync(employee.User.Id);
            return await _employeeRepository.DeleteAsync(id);
        }

        private static EmployeeDto MapToDto(Employee employee)
        {
            return new EmployeeDto
            {
                EmployeeId = employee.Id,
                UserId = employee.UserId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                BirthDate = employee.BirthDate,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive,
                PhotoPath = employee.Photo,
                RoleId = employee.User?.RoleId,
                RoleName = employee.User?.Role?.RoleName,
                UserName = employee.User?.UserName,
            };
        }
    }
}
