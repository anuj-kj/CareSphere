using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Domains.Core;

namespace CareSphere.Services.Users.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(User user, string? password = null, bool register = false);
        Task<User> GetUserByCredential(string usernameOrEmail, string password);
    }
}
