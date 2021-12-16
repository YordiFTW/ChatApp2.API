using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp2.Bussiness.Services
{
    public interface IUserDataService
    {
        Task<User> Register(RegisterModel model);

        Task<string> Login(LoginModel model);

        Task<IQueryable<User>> GetListOfLoggedInUsers();

        Task<IdentityResult> UpdateUser(User user);

        Task<IQueryable<User>> GetAllUsers();

        Task<IdentityResult> BlockUser(UsernameModel model);

        RegisterModel CurrentUser();
    }
}
