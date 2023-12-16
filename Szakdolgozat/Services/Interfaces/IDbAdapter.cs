using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szakdolgozat.Models;

namespace Szakdolgozat.Services.Interfaces
{
    public interface IDbAdapter
    {
        public IEnumerable<Book> GetBooks();
        public Book? GetBook(int bookCode);
        public Task SaveBook(Book book);
        public Task DeleteBook(int bookCode);
        public DatabaseContext GetContext();

    }
}
