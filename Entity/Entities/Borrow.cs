using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
	public class Borrow:BaseEntity
	{
		public string UserId { get; set; }
		public int  BookId { get; set; }
		public Books Book { get; set; }
		public DateTime? BorrowDate { get; set; }=DateTime.Now;
		public DateTime? ReturnDate { get; set; }
		public bool IsReturned { get; set; }

	}
}
