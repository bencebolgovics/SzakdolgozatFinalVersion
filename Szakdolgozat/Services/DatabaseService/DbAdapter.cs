using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Szakdolgozat.Models;
using Szakdolgozat.Services.Interfaces;

namespace Szakdolgozat.Services;

public class DbAdapter : IDbAdapter
{

    private DatabaseContext _dbContext;

    public DbAdapter()
    {
        _dbContext = new DatabaseContext();
    }

    public async Task DeleteBook(int bookCode)
    {
        _dbContext.Remove(GetBook(bookCode));
        await _dbContext.SaveChangesAsync();

    }

    public Book? GetBook(int bookCode)
    {
        return _dbContext.Books?.Find(bookCode);
    }

    public IEnumerable<Book> GetBooks()
    {
        return _dbContext.Books;
    }

    public async Task SaveBook(Book book)
    {
        var newBook = new Book()
        {
            Title = book.Title,
            Author = book.Author,
            BookCode = book.BookCode,
            CoverId = book.CoverId,
            FilePath = book.FilePath,
            PageCount = book.PageCount,
        };

        _dbContext.Add(newBook);
        await _dbContext.SaveChangesAsync();
    }

    public DatabaseContext GetContext()
    {
        return _dbContext;
    }

}
