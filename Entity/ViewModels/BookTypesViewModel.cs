using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
	public class BookTypesViewModel
	{
		public string Name { get; set; }
		public ICollection<Books> Books { get; set; }
	}
}
