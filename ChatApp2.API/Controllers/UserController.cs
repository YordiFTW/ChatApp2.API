using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Domain.Enums;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp2.API.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IUserClaimsPrincipalFactory<User> claimsPrincipalFactory;
        private readonly SignInManager<User> signInManager;

        public UserController(UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> claimsPrincipalFactory,
            SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
            this.signInManager = signInManager;
        }
        [HttpGet]
        [Authorize]
        [Route("IdentityInfo/")]
        public IActionResult IdentityInfo()
        {
            List<string> info = new List<string>();

            info.Add("test");
            foreach (var claim in signInManager.Context.User.Claims)
            {
                info.Add(claim.ToString());
            }


            return Ok(info);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
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

                return Ok("Success");
            }

            return Ok("Not Success");
        }

        [HttpPost]
        [Route("Token/")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password,
                    false, false);
                if (signInResult.Succeeded)
                {
                    return Ok("You are now logged in, " + model.UserName);
                }

                ModelState.AddModelError("", "Invalid UserName or Password");
            }

            return Ok("Invalid Username or Password");
        }


        [HttpPost]
        [Route("ForgotPassword/")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetUrl = Url.Action("ResetPassword", "User",
                        new { token = token, email = user.Email }, Request.Scheme);

                    System.IO.File.WriteAllText("resetLink.txt", resetUrl);
                }

                else
                {
                    //inform user they dont have an account
                }

                return Ok("An email containing the reset password link has been sent");
            }


            return Ok("Error, please fill in the form correctly");
        }

        [HttpPost]
        [Route("ResetPassword/")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return Ok(result.Errors.ToString());
                    }
                    return Ok("Success");
                }
            }

            return Ok("Failure");
        }

        [HttpGet]
        [Route("List/")]
        public async Task<IActionResult> GetListOfLoggedInUsers()
        {


            return Ok(userManager.Users.Where(x => x.IsLoggedIn == false));
        }


        [HttpPut]
        
        public async Task<IActionResult> UpdateUser(User user)
        {
            if (ModelState.IsValid)
            {
                userManager.UpdateAsync(user);
            }


            return Ok("Success");

        }

        [HttpGet]
        [Route("/CurrentUser/")]
        public async Task<IActionResult> CurrentUser()
        {
            User user = userManager.GetUserAsync(User).Result;

            RegisterModel registermodel = new RegisterModel();


            registermodel.EmailAddress = user.Email;
            registermodel.UserName = user.UserName;
            registermodel.Password = "******";
            registermodel.ConfirmPassword = "******";





            return Ok(registermodel);

        }
    }
}