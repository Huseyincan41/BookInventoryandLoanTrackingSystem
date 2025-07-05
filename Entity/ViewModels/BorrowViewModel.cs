using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
	public class BorrowViewModel
	{
        public int BorrowId { get; set; }
        public string BookName { get; set; }
        public string UserId { get; set; }
		public int BookId { get; set; }
		public Books Books { get; set; }
		public DateTime? BorrowDate { get; set; } = DateTime.Now;
		public DateTime? ReturnDate { get; set; }
		public bool IsReturned { get; set; }
	}
}
