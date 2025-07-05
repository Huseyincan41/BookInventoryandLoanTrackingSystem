using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
	public class BookTypes:BaseEntity
	{

        public string Name { get; set; }
		public ICollection<Books> Books { get; set; }
    }
}
