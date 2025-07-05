using Data.Identity;
using Entity.Entities;
using Entity.Services;
using Entity.UnitOfWorks;
using Entity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<BorrowService> _logger;

        public BorrowService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, ILogger<BorrowService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }



        public async Task BorrowBookAsync(int bookId, int userId)
        {
            var bookRepo = _unitOfWork.GetRepository<Books>();
            var borrowRepo = _unitOfWork.GetRepository<Borrow>();

            var book = await _unitOfWork.GetRepository<Books>().GetByIdAsync(bookId);
            if (book == null || book.StockCount <= 0)
                throw new Exception("Book not found or out of stock.");

            book.StockCount -= 1;
            bookRepo.Update(book);

            var borrow = new Borrow
            {
                BookId = book.Id,
                UserId = userId.ToString(), 
                
                BorrowDate = DateTime.Now,
                IsReturned = false
            };

            await borrowRepo.Add(borrow);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation($"Kitap ödünç alındı: KitapID={bookId}, KullanıcıID={userId}");
        }
        public async Task<List<BorrowViewModel>> GetUserBorrowedBooksAsync(int userId)
        {
            var borrowRepo = _unitOfWork.GetRepository<Borrow>();

            var bookRepo = _unitOfWork.GetRepository<Books>();

            var borrows = await borrowRepo.GetAllAsync(
                filter: b => b.UserId == userId.ToString() && !b.IsReturned,
                includeProperties: "Book"
            );

            var result = borrows.Select(b => new BorrowViewModel
            {
                BorrowId = b.Id,
                BookName = b.Book.BookName,
                BorrowDate = b.BorrowDate,
                IsReturned = b.IsReturned
            }).ToList();

            return result;
        }
        public async Task ReturnBookAsync(int borrowId)
        {
            var borrowRepo = _unitOfWork.GetRepository<Borrow>();
            var bookRepo = _unitOfWork.GetRepository<Books>();

            var borrow = await borrowRepo.GetByIdAsync(borrowId);
            if (borrow == null || borrow.IsReturned)
                throw new Exception("Borrow record not found or already returned");

            var book = await bookRepo.GetByIdAsync(borrow.BookId);
            if (book != null)
            {
                book.StockCount += 1;
                bookRepo.Update(book);
            }

            borrow.IsReturned = true;
            borrow.ReturnDate = DateTime.Now;

            
            borrowRepo.Update(borrow);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation($"Kitap iade edildi: BorrowID={borrowId}");
        }
    }
}
