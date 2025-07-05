using Data.Context;
using Data.Identity;
using Data.Repositories;
using Data.UnitOfWorks;
using Entity.Repositories;
using Entity.Services;
using Entity.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Service.Mapping;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Extensions
{
	public static class DependencyExtensions
	{
		public static void AddExtensions(this IServiceCollection services)
		{
			services.AddIdentity<AppUser, AppRole>(
				opt =>
				{
					opt.Password.RequireNonAlphanumeric = false;
					opt.Password.RequiredLength = 3;
					opt.Password.RequireLowercase = false;
					opt.Password.RequireUppercase = false;
					opt.Password.RequireDigit = false;

					//opt.User.RequireUniqueEmail = true;  //Aynı email adresinin girilmesini izinverilmez
					//opt.User.AllowedUserNameCharacters = "abcçdefgğhiıjklmnoöprsştuüvyz"; //kullanıcı adı girilirken izin verilen karakterler.

					opt.Lockout.MaxFailedAccessAttempts = 3;
					opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); //default = 5
				}).AddEntityFrameworkStores<BookDbContext>();

			services.ConfigureApplicationCookie(opt =>
			{
				opt.LoginPath = new PathString("/Account/Login");
				opt.LogoutPath = new PathString("/Account/Logout");
				//opt.AccessDeniedPath = new PathString("/Account/Accessdenied");
				opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
				opt.SlidingExpiration = true; //10 dk dolmadan yeniden login olursa süre baştan başlar.,

				opt.Cookie = new CookieBuilder()
				{
					Name = "Identity.App.Cookie",
					HttpOnly = true,
				};
			});

			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			
			services.AddScoped(typeof(IAccountService), typeof(AccountService));
			services.AddScoped(typeof(IBookService), typeof(BookService));
			services.AddScoped(typeof(IBorrowService), typeof(BorrowService));
			services.AddScoped(typeof(IBookTypesService), typeof(BookTypeService));
			services.AddAutoMapper(typeof(MappingProfile));
		}
	}
}
