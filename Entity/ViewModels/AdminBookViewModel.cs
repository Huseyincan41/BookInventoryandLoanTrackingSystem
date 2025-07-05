using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
    public class AdminBookViewModel
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string Writer { get; set; }
        public DateTime YearOfPublication { get; set; }
        public int BookTypesId { get; set; }
        public string BookTypeName { get; set; }
        public int StockCount { get; set; }
        public List<BookTypes> BookTypesList { get; set; } // SelectList için
    }
}
