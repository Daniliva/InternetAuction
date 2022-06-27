using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace InternetAuction.DAL.Entities.MSSQL
{
    /// <summary>
    /// The user.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets the lots.
        /// </summary>
        /// <value>
        /// The lots.
        /// </value>
        public ICollection<Lot> Lots { get; set; }

        /// <summary>
        /// Gets or Sets the avatars.
        /// </summary>
        public ICollection<ImageId> Avatars { get; set; }

        /// <summary>
        /// Gets or sets the biddings.
        /// </summary>
        public ICollection<Bidding> Biddings { get; set; }

        /// <summary>
        /// Gets or Sets the current avatar.
        /// </summary>
        public ImageId AvatarCurrent { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<RoleUser> UserRoles { get; set; }
    }

    public class Role : IdentityRole
    {
        public virtual ICollection<RoleUser> UserRoles { get; set; }
    }

    public class RoleUser : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}