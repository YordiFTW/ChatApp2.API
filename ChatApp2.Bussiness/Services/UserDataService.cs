using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp2.Bussiness.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly UserManager<User> userManager;
        private readonly HttpContextAccessor httpContextAccessor;
        private readonly SignInManager<User> signInManager;

        public UserDataService(UserManager<User> userManager, HttpContextAccessor httpContextAccessor, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.signInManager = signInManager;
        }

        public RegisterModel CurrentUser()
        {
            User user = userManager.GetUserAsync(httpContextAccessor.HttpContext.User).Result;

            RegisterModel registermodel = new RegisterModel();


            registermodel.EmailAddress = user.Email;
            registermodel.UserName = user.UserName;
            registermodel.Password = "******";
            registermodel.ConfirmPassword = "******";





            return registermodel;
        }

        public async Task<IQueryable<User>> GetListOfLoggedInUsers()
        {
            return (userManager.Users.Where(x => x.IsLoggedIn == true));
        }

        public async Task<IQueryable<User>> GetAllUsers()
        {
            return (userManager.Users);
        }

        public async Task<string> Login(LoginModel model)
        {
            var signInResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password,
                     false, false);
            if (signInResult.Succeeded)
            {
                return ("You are now logged in, " + model.UserName);
            }

            return ("something went wrong");


        }

        public async Task<User> Register(RegisterModel model)
        {            
                var user = await userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    user = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                        Email = model.EmailAddress
                    };

                    var result = await userManager.CreateAsync(user, model.Password);         
                }

            return user;     
        }

        public async Task<IdentityResult> UpdateUser(User user)
        {
            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> BlockUser(UsernameModel model)
        {
            User user = userManager.FindByNameAsync(model.Username).Result;
            user.IsBlocked = true;

            return await userManager.UpdateAsync(user);
        }
    }
}
