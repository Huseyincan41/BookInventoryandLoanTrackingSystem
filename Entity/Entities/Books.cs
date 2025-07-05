using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
	public class Books:BaseEntity
	{
        public String BookName { get; set; }
        public String Writer { get; set; }
        public DateTime YearOfPublication { get; set; }
        public int BookTypesId { get; set; }
        public BookTypes BookTypes { get; set; }
        public int StockCount { get; set; }
    }
}
