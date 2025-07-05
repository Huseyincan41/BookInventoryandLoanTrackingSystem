using Entity.Entities;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Services
{
    public interface IBookTypesService
    {
        Task<IEnumerable<BookTypes>> GetAllAsync();
        Task<BookTypes> GetByIdAsync(int id);
        Task AddAsync(BookTypes bookType);
        Task UpdateAsync(BookTypes bookType);
        Task DeleteAsync(int id);
        //Task<List<SelectListItem>> GetDropdownAsync();
    }
}
