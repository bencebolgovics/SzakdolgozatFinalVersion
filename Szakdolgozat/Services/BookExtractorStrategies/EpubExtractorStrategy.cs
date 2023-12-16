using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Szakdolgozat.Models;
using Szakdolgozat.Services.DatabaseServices;
using Szakdolgozat.Services.FormatServices.Epub;
using Szakdolgozat.Services.Scraping;
using Szakdolgozat.Services.Strategies;

namespace Szakdolgozat.Services.BookExtractorStrategies
{
    public class EpubExtractorStrategy : IBookManipulatorStrategy
    {
        private const string PATH_TO_COVERS = "C:\\Users\\Bence\\Desktop\\Szakdolgozat\\Szakdolgozat\\Szakdolgozat\\Resources\\CoverImages\\";
        private readonly Regex regex = new Regex("\\[eBook #[0-9]+\\]", RegexOptions.IgnoreCase);

        private DbAdapter _adapter;
        private GutenbergEbookMetadataProvider _metaDataProvider;

        public EpubExtractorStrategy()
        {
            _adapter = DbProvider.GetInstance();
            _metaDataProvider = new();
        }

        public async void ExtractingAndSaving(string pathWithFileName)
        {
            Epub epub = EpubReader.Read(pathWithFileName);

            var text = epub.ToText();

            var match = regex.Match(text).Captures.First().Value;
            var bookCode = int.Parse(match.Remove(match.Length - 1)
                                          .Split('#')
                                          .Last());

            if (_adapter.GetBook(bookCode) != null)
                return;

            string imgPath = GetImagePath(pathWithFileName, epub);
            string title = epub.Title;
            string author = epub.Authors.Aggregate("",
                                (current, next) => current + " " + next);
            int pageCount = text.Length / 1024;

            Book bookToSave = new Book(bookCode, title, pathWithFileName, author, pageCount, imgPath);

            await _adapter.SaveBook(bookToSave);
        }

        private static string GetImagePath(string pathWithFileName, Epub epub)
        {
            string imgPath;
            if (epub.CoverImage != null)
            {
                var imgSaving = pathWithFileName.Split('\\').Last().Split('.')[0];

                imgPath = PATH_TO_COVERS + imgSaving + ".png";

                if (File.Exists(imgPath))
                    return imgPath;

                var coverImg = Image.FromStream(new MemoryStream(epub.CoverImage));

                coverImg.Save(imgPath, ImageFormat.Png);
            }
            else
            {
                imgPath = PATH_TO_COVERS + "NoCover.jpg";
            }

            return imgPath;
        }

        public void DeleteBookResources(Book book)
        {
            if (File.Exists(book.CoverId) && book.CoverId != $"{PATH_TO_COVERS}//NoCover.jpg")
                File.Delete(book.CoverId);
        }
    }
}
