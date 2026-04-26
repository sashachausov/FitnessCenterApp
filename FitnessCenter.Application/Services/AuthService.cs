using FitnessCenter.Application.Interfaces;
using FitnessCenter.Application.Interfaces.Repositories;
using FitnessCenter.Application.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto?> AuthenticateAsync(string userName, string password)
        {
            var user = await _userRepository.GetByUserNameWithRoleAsync(userName);
            if (user == null) return null;

            bool passwordValid = _passwordHasher.VerifyPassword(password, user.PasswordHash);
            if (!passwordValid) return null;

            return new UserDto
            {
                UserId = user.Id,
                UserName = userName,
                RoleID = user.RoleId,
                RoleName = user.Role?.RoleName ?? "Неизвестно",
                HasFullAccess = user.Role?.HasFullAccess ?? false,
            };
        }
    }
}
