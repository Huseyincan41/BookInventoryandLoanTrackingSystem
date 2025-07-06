using Entity.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
	public class BookViewModel
	{
        public int Id { get; set; }
        public String BookName { get; set; }
		public String Writer { get; set; }
		public DateTime YearOfPublication { get; set; }
        public string BookTypeName { get; set; }
        //public BookTypes BookTypes { get; set; }
		public int StockCount { get; set; }
        public int BookTypesId { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<BookTypes> AllBookTypes { get; set; }

    }
}
