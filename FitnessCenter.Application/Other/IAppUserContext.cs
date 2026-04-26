using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Other
{
    public interface IAppUserContext
    {
        UserDto? CurrentUser { get; }
        bool IsAuthenticated { get; }
        bool HasFullAccess { get; }

        void SetCurrentUser(UserDto? currentUser);
        void ClearCurrentUser();
    }
}
