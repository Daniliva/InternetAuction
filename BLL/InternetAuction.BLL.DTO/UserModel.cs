using System.Collections.Generic;

namespace InternetAuction.BLL.DTO
{
    public class UserModel
    {
        public string Id { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public ICollection<RoleUserModel> UserRoles { get; set; }

        public byte[] AvatarCurrent { get; set; }
    }
}