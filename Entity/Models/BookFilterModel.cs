using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class BookFilterModel
    {
        public string? Search { get; set; }
        public string? WriterFilter { get; set; }
        public int? BookTypeId { get; set; }
        public string? StockOrder { get; set; }  // "asc" veya "desc"
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
