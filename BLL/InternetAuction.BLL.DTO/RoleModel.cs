using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetAuction.BLL.DTO
{
    public class RoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public ICollection<UserModel> Users { get; set; }
    }
}