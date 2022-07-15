using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using InternetAuction.BLL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace InternetAuction.WEB.Domain
{

    /// <summary>
    /// The user change info.
    /// </summary>
    public class UserChangeInfo : UserModel
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="UserChangeInfo"/> class.
        /// </summary>
        public UserChangeInfo()
		{
			AllRoleList = new List<string>();
			RoleUsers = new List<RoleUserModel>();
		}
        /// <summary>
        /// Gets or sets all role list.
        /// </summary>
        /// <value>
        /// All role list.
        /// </value>
        [BindProperty]
		public IList<string> AllRoleList { get; set; }
	}
}