using Entity.Entities;
using Entity.Services;
using Entity.UnitOfWorks;
using Entity.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BookTypeService : IBookTypesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BookTypes>> GetAllAsync()
        {
            return await _unitOfWork.GetRepository<BookTypes>().GetAllAsync();
        }

        public async Task<BookTypes> GetByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository<BookTypes>().GetByIdAsync(id);
        }

        public async Task AddAsync(BookTypes bookType)
        {
            await _unitOfWork.GetRepository<BookTypes>().Add(bookType);
            await _unitOfWork.CommitAsync();

        }

        public async Task UpdateAsync(BookTypes bookType)
        {
            _unitOfWork.GetRepository<BookTypes>().Update(bookType);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<BookTypes>();
            var bookType = await repo.GetByIdAsync(id);
            if (bookType != null)
            {
                repo.Delete(bookType);
                await _unitOfWork.CommitAsync();
            }
        }
        public async Task<List<SelectListItem>> GetDropdownAsync()
        {
            var types = await _unitOfWork.GetRepository<BookTypes>().GetAllAsync();
            return types.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();
        }
    }
}
