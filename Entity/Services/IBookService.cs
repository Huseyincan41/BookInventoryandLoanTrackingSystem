using Entity.Entities;
using Entity.Models;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Services
{
    public interface IBookService
    {
        Task<List<BookViewModel>> GetAllAsync();
        Task<BookViewModel> GetByIdAsync(int id);
        Task<IList<BookViewModel>> GetAllAsyncByPaging(int currentPage, int pageSize);
        Task AddAsync(BookViewModel model);
        Task UpdateAsync(BookViewModel model);
        Task DeleteAsync(int id);
        Task<PagedViewModel<BookViewModel>> GetFilteredBooksAsync(BookFilterModel filter);
    }
}
