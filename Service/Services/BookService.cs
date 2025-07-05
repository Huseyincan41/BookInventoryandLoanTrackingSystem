using Data.Context;
using Entity.Entities;
using Entity.Services;
using Entity.UnitOfWorks;
using Entity.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookService> _logger;
        public BookService(IUnitOfWork unitOfWork, ILogger<BookService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<BookViewModel>> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<Books>();
            var books = await repo.GetAllAsync(includeProperties: "BookTypes");

            return books.Select(b => new BookViewModel
            {
                Id = b.Id,
                BookName = b.BookName,
                Writer = b.Writer,
                YearOfPublication = b.YearOfPublication,
                BookTypesId = b.BookTypesId,
                BookTypeName = b.BookTypes.Name,
                StockCount = b.StockCount
            }).ToList();
        }

        public async Task<BookViewModel> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Books>();
            var book = await repo.GetByIdAsync(id);
            var bookTypesRepo = _unitOfWork.GetRepository<BookTypes>();
            var allTypes = await bookTypesRepo.GetAllAsync();

            return new BookViewModel
            {
                Id = book.Id,
                BookName = book.BookName,
                Writer = book.Writer,
                YearOfPublication = book.YearOfPublication,
                BookTypesId = book.BookTypesId,
                StockCount = book.StockCount,
                AllBookTypes = allTypes.ToList()
            };
        }

        public async Task AddAsync(BookViewModel model)
        {
           
            var repo = _unitOfWork.GetRepository<Books>();
            var newBook = new Books
            {
                Id = model.Id,
                BookName = model.BookName,
                Writer = model.Writer,
                YearOfPublication = model.YearOfPublication,
                BookTypesId = model.BookTypesId,
                StockCount = model.StockCount,
                
            };
            await repo.Add(newBook);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation($"Kitap eklendi: {model.BookName} - ID: {model.Id}");
        }

        public async Task UpdateAsync(BookViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Books>();
            var book = await repo.GetByIdAsync(model.Id);
            book.BookName = model.BookName;
            book.Writer = model.Writer;
            book.YearOfPublication = model.YearOfPublication;
            book.BookTypesId = model.BookTypesId;
            book.StockCount = model.StockCount;

            repo.Update(book);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation($"Kitap güncellendi: {model.BookName} - ID: {model.Id}");
        }

        public async Task DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Books>();
            repo.Delete(id);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation($"Kitap silindi - ID: {id}");
        }
    }
}
