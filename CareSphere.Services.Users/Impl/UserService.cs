using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.Interfaces;
using CareSphere.Domains.Core;
using CareSphere.Services.Users.Interfaces;

namespace CareSphere.Services.Users.Impl
{
    public class UserService: IUserService
    {
        private ICareSphereUnitOfWork UnitOfWork { get; set; }

        public UserService(ICareSphereUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public async Task<User> CreateUser(User user, string? password=null, bool register=false)
        {
            var userRepository = UnitOfWork.Repository<User>();
            // Check if user with the same email already exists
            var existingUser = await userRepository.GetByPropertyAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                if (register)
                {
                    throw new ArgumentException("User with the same email already exists");
                }
                return existingUser;
            }

            // Generate username if not provided
            if (string.IsNullOrEmpty(user.Username))
            {
                user.Username = user.Email;
            }
            if(user.CreatedAt == default)
            {
                user.CreatedAt = DateTime.UtcNow;
            }

            // Generate password hash if not provided
            if (string.IsNullOrEmpty(password))
            {
                password = "defaultPassword"; // Replace "defaultPassword" with a secure default password or generate one               
            }
            user.PasswordHash = GeneratePasswordHash(password);
            user.Role = "User"; // Replace "User" with the default role for new users
            await userRepository.AddAsync(user);
            await UnitOfWork.CommitAsync();
            return user;
        }
        public async Task<User> GetUserByCredential(string usernameOrEmail, string password)
        {
            var userRepository = UnitOfWork.Repository<User>();
            var user= await userRepository.GetByPropertyAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
            if(user == null)
            {
                throw new ArgumentException("User not found");
            }
            var passwordHash = GeneratePasswordHash(password);
            if (user.PasswordHash != passwordHash)
            {
                throw new ArgumentException("Invalid password");
            }
            return user;
        }



        private string GeneratePasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
