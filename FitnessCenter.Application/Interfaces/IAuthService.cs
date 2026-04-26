using FitnessCenter.Application.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> AuthenticateAsync(string userName, string password);
    }
}
