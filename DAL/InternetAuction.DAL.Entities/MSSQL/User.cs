using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace InternetAuction.DAL.Entities.MSSQL
{
	/// <summary>
	/// The user.
	/// </summary>
	public class User
	{
		public string Id { get; set; }

		public virtual string UserName { get; set; }

		/// <summary>
		/// Gets or sets the email address for this user.
		/// </summary>

		public virtual string Email { get; set; }

		/// <summary>
		/// Gets or sets a salted and hashed representation of the password for this user.
		/// </summary>
		public virtual string PasswordHash { get; set; }

		/// <summary>
		/// Gets or sets the lots.
		/// </summary>
		/// <value>
		/// The lots.
		/// </value>
		public virtual ICollection<Lot> Lots { get; set; }

		/// <summary>
		/// Gets or Sets the avatars.
		/// </summary>
		public ICollection<ImageId> Avatars { get; set; }

		public virtual ICollection<Bidding> Biddings { get; set; }

		/// <summary>
		/// Gets or Sets the current avatar.
		/// </summary>
		public ImageId AvatarCurrent { get; set; }

		/*   public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
           public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
           public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }*/

		/// <summary>
		/// Gets or sets the user roles.
		/// </summary>
		//	public virtual List<Role> Roles { get; set; } = new List<Role>();

		public virtual ICollection<RoleUser> RoleUsers { get; set; }
	}

	public class Role
	{      /// <summary>
		   /// Gets or sets the primary key for this role.
		   /// </summary>
		public virtual string Id { get; set; }

		/// <summary>
		/// </summary>
		public virtual string Name { get; set; }

		//	public virtual List<User> Users { get; set; } = new List<User>() { };
		public virtual ICollection<RoleUser> RoleUsers { get; set; }
	}

	public class RoleUser
	{
		public string RolesId { get; set; }
		public string UsersId { get; set; }

		public virtual Role Roles { get; set; }
		public virtual User Users { get; set; }
	}
}