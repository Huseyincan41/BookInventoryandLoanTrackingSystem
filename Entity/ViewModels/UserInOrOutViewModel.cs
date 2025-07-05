using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
	public class UserInOrOutViewModel
	{
		public RoleViewModel Role { get; set; }
		public List<UserViewModel> UsersInRole { get; set; }
		public List<UserViewModel> UsersOutRole { get; set; }
	}
}
