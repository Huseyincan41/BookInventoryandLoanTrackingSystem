﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
	public class UserViewModel
	{
		public int Id { get; set; }
		//[Required(ErrorMessage = "İsim alanı boş geçilemez")]
		public string Name { get; set; }
		//[Required(ErrorMessage = "Soy isim alanı boş geçilemez")]
		public string Surname { get; set; }

		//[Required(ErrorMessage = "Kullanıcı adı boş geçilemez")]
		public string UserName { get; set; }
		//[Required(ErrorMessage = "Telefon alanı boş geçilemez")]
		public string PhoneNumber { get; set; }
		//[Required(ErrorMessage = "Email alanı boş geçilemez")]
		//[EmailAddress(ErrorMessage = "Email formatına uygun değil")]
		public string Email { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }
		public string? Address { get; set; }
	}
}
