using System;
using System.ComponentModel.DataAnnotations;

namespace Szakdolgozat.Models
{
    public class Book
    {
        [Key]
        public int BookCode { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }
        public string CoverId { get; set; }

        public Book(int bookCode, string title, string filePath, string author, int pageCount, string coverId)
        {
            BookCode = bookCode;
            Title = title;
            Author = author;
            PageCount = pageCount;
            CoverId = coverId;
            FilePath = filePath;
        }

        public Book()
        {
            BookCode = new Random().Next(50000);
            Title = string.Empty;
            Author = string.Empty;
            PageCount = 0;
            CoverId = string.Empty;
            FilePath = string.Empty;
        }
    }
}
