using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using InternetAuction.BLL.Contract;
using InternetAuction.BLL.DTO;
using InternetAuction.WEB.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace InternetAuction.WEB.Pages.Controllers
{
    /// <summary>
    /// The user controller.
    /// </summary>

    [Authorize]
    public class UserController : Controller
    {
        private readonly IExpansionGetEmail<UserModel, string> userService;
        private ICrud<RoleUserModel, string> roleUserService;
        private ICrud<RoleModel, string> roleService;

        public UserController(IExpansionGetEmail<UserModel, string> userService, ICrud<RoleModel, string> roleService, ICrud<RoleUserModel, string> roleUserService)
        {
            this.roleService = roleService;
            this.roleUserService = roleUserService;
            this.userService = userService;
            //   Method();
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var nameUser = HttpContext.User.Identity.Name;

            return View(await userService.GetByEmail(nameUser));
        }

        /*	public async Task<ActionResult> Details(string id)
			{
				return View(await userService.GetByIdAsync(id));
			}*/

        [Authorize]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await userService.GetByIdAsync(id);
            var userChangeItem = new UserChangeInfo();
            if (user != null)
            {
                userChangeItem.Id = user.Id;
                userChangeItem.Email = user.Email;
                userChangeItem.RoleUsers = user.RoleUsers;
                userChangeItem.PasswordHash = user.PasswordHash;
                userChangeItem.UserName = user.UserName;
                var allRoles = roleService.GetAllAsync().Result.Select(x => x.Name);

                foreach (var role in allRoles)
                {
                    userChangeItem.AllRoleList.Add(role);
                }
                return View(userChangeItem);
            }

            return NotFound();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, List<string> roles)
        {
            try
            {
                var user = await userService.GetByIdAsync(id);
                var rolesColl = await roleService.GetAllAsync();

                user.RoleUsers = new List<RoleUserModel>() { };

                foreach (var value in rolesColl)
                {
                    if (roles.Any(x => x == value.Name) && user.RoleUsers.All(x => x.Roles.Name != value.Name))
                    {
                        RoleUserModel roleUserModel = new RoleUserModel
                        {
                            Users = user,
                            Roles = value
                        };
                        user.RoleUsers.Add(roleUserModel);
                    }
                }
                await userService.UpdateAsync(user);

                return RedirectToAction(nameof(ListAsync));
            }
            catch (Exception exception)
            {
                var user = await userService.GetByIdAsync(id);
                if (user != null)
                    return View(user);
                else
                    return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditPersonalData(string id)
        {
            var user = await userService.GetByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPersonalData(string id, UserModel collection)
        {
            try
            {
                var user = await userService.GetByIdAsync(id);
                user.UserName = collection.UserName;
                user.PasswordHash = collection.PasswordHash;
                user.Email = collection.Email;
                await userService.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                var user = await userService.GetByIdAsync(id);
                return View(user);
            }
        }

        /*
		public ActionResult Delete(int id)
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}*/

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ListAsync()
        {
            return View(await userService.GetAllAsync());
        }
    }
}