using AutoMapper;
using Data.Identity;
using Entity.Entities;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mapping
{
	public class MappingProfile:Profile
	{
		public MappingProfile() 
		{
			CreateMap<AppUser, UserViewModel>().ReverseMap();
			CreateMap<AppUser, LoginViewModel>().ReverseMap();
			CreateMap<AppUser, RegisterViewModel>().ReverseMap();
			CreateMap<AppRole, RoleViewModel>().ReverseMap();
			CreateMap<Books, BookViewModel>().ReverseMap();
			CreateMap<BookTypes, BookTypesViewModel>().ReverseMap();
			CreateMap<Borrow, BorrowViewModel>().ReverseMap();

		}
	}
}
