using System;
using InternetAuction.BLL.Contract;
using InternetAuction.BLL.DTO;
using InternetAuction.WEB.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using System.Linq;

namespace InternetAuction.WEB.Pages.Controllers
{
    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IExpansionGetEmail<UserModel, string> userService;
        private readonly ICrud<RoleUserModel, string> roleUserService;
        private readonly ICrud<RoleModel, string> roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="roleUserService">The role user service.</param>
        public AccountController(IExpansionGetEmail<UserModel, string> userService, ICrud<RoleModel, string> roleService, ICrud<RoleUserModel, string> roleUserService)
        {
            this.roleService = roleService;
            this.roleUserService = roleUserService;
            this.userService = userService;
        }

        /*
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }*/

        /// <summary>
        /// Logs the out.
        /// </summary>
        /// <returns>A Task.</returns>
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var g = HttpContext.User.Identity;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn", "Account");
        }

        /// <summary>
        /// Registrations the.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Get information for registration and registration user in system with role "User"
        /// Redirect to login method, if registration was successful or return form if not
        /// </summary>
        /// <param name="collection"></param>dapashlivy9966@gmail
        /// <returns></returns>99885544776611
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(UserModel collection)
        {
            try
            {
                RoleUserModel roleUserModel = new RoleUserModel();
                roleUserModel.Users = collection;
                roleUserModel.Roles = roleService.GetByIdAsync("1").Result;

                RoleUserModel roleUserModel1 = new RoleUserModel();
                roleUserModel1.Users = collection;
                roleUserModel1.Roles = roleService.GetByIdAsync("2").Result;
                collection.RoleUsers = new List<RoleUserModel>() { roleUserModel, roleUserModel1 };
                await userService.AddAsync(collection);
                var result = userService.GetByEmail(collection.Email).Result;
                return (RedirectToAction(nameof(LogIn)));
            }
            catch (Exception e)
            {
                return (View(collection));
            }
        }

        public ActionResult LogIn()
        {
            return View();
        }

        /// <summary>
        /// Enter user in system or return form
        /// </summary>
        /// <param name="collection">Entities consists of login and password</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(LoginInfo collection)
        {
            try
            {
                UserModel user = await userService.GetByEmail(collection.Login);
                if (user != null & (user.PasswordHash == collection.Password && ModelState.IsValid))
                {
                    await Authenticate(user);
                }
                else { ModelState.AddModelError("", "Wrong username and/or password"); }

                return RedirectToAction("Index", "User");
            }
            catch
            {
                return View(collection);
            }
        }

        private async Task Authenticate(UserModel user)
        {
            List<ClaimsIdentity> listClaimsIdentities = new List<ClaimsIdentity>();

            foreach (var role in user.RoleUsers)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Roles.Name)
            };
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                listClaimsIdentities.Add(id);
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(listClaimsIdentities));
        }
    }
}