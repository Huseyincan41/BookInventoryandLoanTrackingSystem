using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Services
{
    public interface IBorrowService
    {
        Task BorrowBookAsync(int bookId, int userId);
        Task<List<BorrowViewModel>> GetUserBorrowedBooksAsync(int userId);
        Task ReturnBookAsync(int borrowId);
    }
}
