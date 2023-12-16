using Aspose.Words;
using Szakdolgozat.Models;
using Szakdolgozat.Models.Enum;

namespace Szakdolgozat.Services.ConverterService
{
    public static class Converter
    {
        public static void Convert(this Book book, string savePath, string extension)
        {
            switch (extension.GetFormatEnum())
            {
                case Format.Mobi:
                    ConvertToMobi(book, savePath, extension);
                    break;
                case Format.Epub:
                    ConvertToEpub(book, savePath, extension);
                    break;
            }
        }

        private static void ConvertToMobi(Book book, string savePath, string extension)
        {
            Document doc = new Document(book.FilePath);

            doc.Save(savePath, SaveFormat.Mobi);
        }

        private static void ConvertToEpub(Book book, string savePath, string extension)
        {
            Document doc = new Document(book.FilePath);

            doc.Save(savePath, SaveFormat.Epub);
        }
    }
}
