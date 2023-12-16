using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Szakdolgozat.Models;
using Szakdolgozat.Services.EventManager;

namespace Szakdolgozat.Services.Extensions
{
    public class BookJsonExtensions : SerializeJsonBase
    {
        public BookJsonExtensions()
        {
            Target = "C:\\Users\\Bence\\Documents\\Reader\\Jsons\\LastReadBook.json";
        }

        public void SerializeBookToJson(Book book)
        {
            var bookJson = new BookJson(book.BookCode,
                                        book.Title,
                                        book.FilePath,
                                        book.Author,
                                        book.PageCount,
                                        book.CoverId);

            Serialize(bookJson);
        }

        public override void Serialize<T>(T obj)
        {
            var jsonList = new List<T>(1)
            {
                obj
            };

            File.WriteAllText(Target, JsonConvert.SerializeObject(jsonList, Formatting.Indented));

            Events.RaiseJsonChanged();
        }

        public BookJson[] DeserializeJson()
        {
            return Deserialize<BookJson>();
        }

        public Book? GetLastReadBookFromJson()
        {
            var bookJson = DeserializeJson().FirstOrDefault();

            if (bookJson == null)
                return null;

            return new Book()
            {
                BookCode = bookJson.BookCode,
                Title = bookJson.Title,
                Author = bookJson.Author,
                PageCount = bookJson.PageCount,
                CoverId = bookJson.CoverId,
                FilePath = bookJson.FilePath
            };
        }

        public void DeleteLastReadBook(int bookCode)
        {
            var bookJson = DeserializeJson().FirstOrDefault(x => x.BookCode == bookCode);

            if (bookJson == null)
                return;

            Serialize(new Book() { BookCode = -1});

            Events.RaiseJsonChanged();
        }
    }
}
