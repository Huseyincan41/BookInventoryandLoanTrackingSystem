using Data.Context;
using Entity.Entities;
using Entity.Models;
using Entity.Services;
using Entity.UnitOfWorks;
using Entity.ViewModels;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookService(IUnitOfWork unitOfWork, ILogger<BookService> logger, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
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
                StockCount = b.StockCount,
                ImagePath = b.ImagePath,

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
                ImagePath = book.ImagePath,
                AllBookTypes = allTypes.ToList()
            };
        }

        public async Task AddAsync(BookViewModel model)
        {
           
            var repo = _unitOfWork.GetRepository<Books>();
            string imagePath = null;
            if (model.ImageFile != null)
            {
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var path = Path.Combine(wwwRootPath, "images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                imagePath = "/images/" + fileName;
            }
            var newBook = new Books
            {
                Id = model.Id,
                BookName = model.BookName,
                Writer = model.Writer,
                YearOfPublication = model.YearOfPublication,
                BookTypesId = model.BookTypesId,
                StockCount = model.StockCount,
                ImagePath = imagePath,
                
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
            book.ImagePath = model.ImagePath;
            if (model.ImageFile != null)
            {
                var wwwRootPath = _webHostEnvironment.WebRootPath;

                // Eski resmi sil
                if (!string.IsNullOrEmpty(book.ImagePath))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, book.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Yeni resmi kaydet
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var newImagePath = Path.Combine(wwwRootPath, "images", fileName);
                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                book.ImagePath = "/images/" + fileName;
            }
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
        public async Task<IList<BookViewModel>> GetAllAsyncByPaging(int currentPage, int pageSize)
        {
            var books = await _unitOfWork.GetRepository<Books>()
                .GetAllByPagingAsync(
                    predicate: null,
                    include: q => q.Include(b => b.BookTypes),
                    order: q => q.OrderBy(b => b.StockCount),
                    enableTracking: false,
                    currentPage: currentPage,
                    pageSize: pageSize);

            return books.Select(b => new BookViewModel
            {
                Id = b.Id,
                BookName = b.BookName,
                Writer = b.Writer,
                YearOfPublication = b.YearOfPublication,
                BookTypesId = b.BookTypesId,
                BookTypeName = b.BookTypes.Name,
                StockCount = b.StockCount,
                ImagePath = b.ImagePath,
            }).ToList();
        }
        public async Task<PagedViewModel<BookViewModel>> GetFilteredBooksAsync(BookFilterModel filter)
        {
            var query = _unitOfWork.GetRepository<Books>()
                .GetQueryable(include: q => q.Include(b => b.BookTypes), enableTracking: false);

            // Filtreleme
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(b => b.BookName != null && b.BookName.Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.WriterFilter))
            {
                query = query.Where(b => b.Writer != null && b.Writer.Contains(filter.WriterFilter));
            }

            if (filter.BookTypeId.HasValue && filter.BookTypeId.Value != 0)
            {
                query = query.Where(b => b.BookTypesId == filter.BookTypeId.Value);
            }

            // Sıralama
            if (filter.StockOrder == "asc")
            {
                query = query.OrderBy(b => b.StockCount);
            }
            else if (filter.StockOrder == "desc")
            {
                query = query.OrderByDescending(b => b.StockCount);
            }

            // Sayfalama
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    BookName = b.BookName,
                    Writer = b.Writer,
                    YearOfPublication = b.YearOfPublication,
                    BookTypesId = b.BookTypesId,
                    BookTypeName = b.BookTypes.Name,
                    StockCount = b.StockCount,
                    ImagePath = b.ImagePath
                })
                .ToListAsync();

            return new PagedViewModel<BookViewModel>
            {
                Items = items,
                CurrentPage = filter.CurrentPage,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

    }
}
