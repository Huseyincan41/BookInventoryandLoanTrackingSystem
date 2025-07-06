using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
    public class PagedViewModel<T>
    {
        public IList<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
