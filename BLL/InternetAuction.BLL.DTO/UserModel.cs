using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetAuction.BLL.DTO
{
	public class UserModel
	{
		public string Id { get; set; }

		[Required(ErrorMessage = "Enter Password")]
		[StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string PasswordHash { get; set; }

		[Required(ErrorMessage = "Enter Login")]
		[StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Enter UserName")]
		[StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		public ICollection<RoleUserModel> RoleUsers { get; set; }
		public  ICollection<LotModel> Lots { get; set; }

		//    public byte[] AvatarCurrent { get; set; }
	}
}