using AutoMapper;
using Data.Identity;
using Entity.Services;
using Entity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IMapper _mapper;

		public AccountService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IMapper mapper)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_mapper = mapper;
		}

		public async Task<string> CreateRoleAsync(RoleViewModel model)
		{
			string message = string.Empty;
			AppRole role = new AppRole()
			{
				Name = model.Name,
				Description = model.Description
			};
			var identityResult = await _roleManager.CreateAsync(role);

			if (identityResult.Succeeded)
			{
				message = "OK";
			}
			else
			{
				foreach (var error in identityResult.Errors)
				{
					message = error.Description;
				}
			}
			return message;
		}

		public async Task<string> CreateUserAsync(RegisterViewModel model)
		{
			string message = string.Empty;
			AppUser user = new AppUser()
			{
				Name = model.FirstName,
				Surname = model.LastName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				UserName = model.UserName,

			};
			var idendtityResult = await _userManager.CreateAsync(user, model.ConfirmPassword);
			if (idendtityResult.Succeeded)
			{
				message = "OK";
			}
			else
			{
				foreach (var item in idendtityResult.Errors)
				{
					message = item.Description;
				}
			}
			return message;
		}

		public async Task<UserViewModel> Find(string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			return _mapper.Map<UserViewModel>(user);
		}

		public async Task<string> FindByNameAsync(LoginViewModel model)
		{
			string message = string.Empty;
			var user = await _userManager.FindByNameAsync(model.UserName);
			if (user == null)
			{
				message = "Kullanıcı bulunamadı!";
				return message;
			}
			var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
			if (signInResult.Succeeded) { message = "OK"; }
			return message;
		}

		public async Task<List<RoleViewModel>> GetAllRoles()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return _mapper.Map<List<RoleViewModel>>(roles);
		}

		public async Task SignOutAsync()
		{
			await _signInManager.SignOutAsync();
		}
		public async Task<List<UserViewModel>> GetAllUsers()
		{
			var users = await _userManager.Users.ToListAsync();
			return _mapper.Map<List<UserViewModel>>(users);
		}

		public async Task<UserInOrOutViewModel> GetAllUsersWithRole(string id)
		{
			var role = await FindRoleByIdAsync(id);
			var usersInRole = new List<AppUser>();
			var usersOutRole = new List<AppUser>();
			var users = await _userManager.Users.ToListAsync();
			foreach (var user in users)
			{
				if (await _userManager.IsInRoleAsync(user, role.Name))
				{
					usersInRole.Add(user);//bu rolde bulunan kullanıcıların listesi
				}
				else
				{
					usersOutRole.Add(user);//bu rolde olmayan kullanıcıların listesi
				}
			}
			UserInOrOutViewModel model = new UserInOrOutViewModel()
			{
				
				Role = role,
				UsersInRole = _mapper.Map<List<UserViewModel>>(usersInRole),
				UsersOutRole = _mapper.Map<List<UserViewModel>>(usersOutRole)

			};
			return model;
		}

		public async Task<RoleViewModel> FindRoleByIdAsync(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			return _mapper.Map<RoleViewModel>(role);
		}

		public async Task<string> EditRoleListAsync(EditRoleViewModel model)
		{
			string msg = "OK";
			foreach (var userId in model.UserIdsToAdd ?? new string[] { })
			{
				var user = await _userManager.FindByIdAsync(userId);
				if (user != null)
				{
					var result = await _userManager.AddToRoleAsync(user, model.RoleName);
					if (!result.Succeeded)
					{
						msg = $"{user.UserName} role eklenemedi";
					}
				}

			}
			foreach (var userId in model.UserIdsToDelete ?? new string[] { })
			{
				var user = await _userManager.FindByIdAsync(userId);
				if (user != null)
				{
					var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
					if (!result.Succeeded)
					{
						msg = $"{user.UserName} rolden çıkarılamadı";
					}
				}

			}
			return msg;
		}
        public async Task<UserViewModel> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;

            return new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                
            };
        }

        public async Task<string> UpdateUserAsync(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null) return "Kullanıcı bulunamadı!";

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? "OK" : string.Join(", ", result.Errors.Select(e => e.Description));
        }

        public async Task<string> DeleteUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return "Kullanıcı bulunamadı!";

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? "OK" : string.Join(", ", result.Errors.Select(e => e.Description));
        }
    }
}
