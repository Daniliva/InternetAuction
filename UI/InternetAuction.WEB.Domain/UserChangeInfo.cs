using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using InternetAuction.BLL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace InternetAuction.WEB.Domain
{
	public class UserChangeInfo : UserModel
	{
		public UserChangeInfo()
		{
			AllRoleList = new List<string>();
			RoleUsers = new List<RoleUserModel>();
		}

		[BindProperty]
		public IList<string> AllRoleList { get; set; }
	}
}