using FitnessCenter.Application.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Infrastructure.Other
{
    public class AppUserContext : IAppUserContext
    {
        public UserDto? CurrentUser { get; private set; }
        public bool IsAuthenticated => CurrentUser != null;
        public bool HasFullAccess => CurrentUser?.HasFullAccess == true;

        public void SetCurrentUser(UserDto? currentUser)
        {
            CurrentUser = currentUser;
        }

        public void ClearCurrentUser()
        {
            CurrentUser = null;
        }        
    }
}
