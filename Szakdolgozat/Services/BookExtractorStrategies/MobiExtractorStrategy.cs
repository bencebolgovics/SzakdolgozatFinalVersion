using Aspose.Words;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Szakdolgozat.Models;
using Szakdolgozat.Services.DatabaseServices;
using Szakdolgozat.Services.Strategies;

namespace Szakdolgozat.Services.BookExtractorStrategies
{
    public class MobiExtractorStrategy : IBookManipulatorStrategy
    {
        private const string PATH_TO_COVERS = "C:\\Users\\Bence\\Desktop\\Szakdolgozat\\Szakdolgozat\\Szakdolgozat\\Resources\\CoverImages\\";

        private DbAdapter _adapter;

        public MobiExtractorStrategy()
        {
            _adapter = DbProvider.GetInstance();
        }

        public void DeleteBookResources(Book book)
        {
            if (File.Exists(book.CoverId) && book.CoverId != $"{PATH_TO_COVERS}//NoCover.jpg")
                File.Delete(book.CoverId);
        }

        public async void ExtractingAndSaving(string path)
        {
            Document document = new Document(path);

            string imgPath = GetImagePath(document);
            string title = string.IsNullOrEmpty(document.BuiltInDocumentProperties.Title) ? "<unknown>" : document.BuiltInDocumentProperties.Title;
            string author = document.BuiltInDocumentProperties.Author;
            int pageCount = document.BuiltInDocumentProperties.Pages;
            int bookCode = new Random().Next(1000);

            Book bookToSave = new Book(bookCode, title, path, author ?? "anonim", pageCount, imgPath);

            await _adapter.SaveBook(bookToSave);
        }

        private static string GetImagePath(Document document)
        {
            string imgPath;
            if (document.BuiltInDocumentProperties.Thumbnail.Length != 0)
            {
                imgPath = PATH_TO_COVERS + document.BuiltInDocumentProperties.Title + ".png";

                if (File.Exists(imgPath))
                    return imgPath;

                var coverImg = Image.FromStream(new MemoryStream(document.BuiltInDocumentProperties.Thumbnail));

                coverImg.Save(imgPath, ImageFormat.Png);
            }
            else
            {
                imgPath = PATH_TO_COVERS + "NoCover.jpg";
            }

            return imgPath;
        }
    }
}
