using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Szakdolgozat.Models;
using Szakdolgozat.Models.Constants;
using Szakdolgozat.Models.Enum;
using Szakdolgozat.Services.BookExtractorStrategies;
using HttpClient = System.Net.Http.HttpClient;

namespace Szakdolgozat.Services.Scraping
{
    public sealed class GutenbergDownloader
    {
        private const string PATH_TO_BOOKS = "C:\\Users\\Bence\\Documents\\Reader\\";

        private SearchResultDTO _toBeDownloaded;

        public GutenbergDownloader(SearchResultDTO toBeDownloaded)
        {
            _toBeDownloaded = toBeDownloaded;
        }

        public async void ImportBook()
        {
            GutenbergEbookMetadataProvider provider = new GutenbergEbookMetadataProvider();

            await DownloadBook();

            SaveToDatabase();
        }

        private void SaveToDatabase()
        {
            string filePath = PATH_TO_BOOKS + _toBeDownloaded.Title + "." + Format.Epub.GetString();

            var strategy = ExtractorContext.GetStrategy(Format.Epub);

            strategy.ExtractingAndSaving(filePath);
        }

        private async Task DownloadBook()
        {
            if (!Directory.Exists(PATH_TO_BOOKS))
                Directory.CreateDirectory(PATH_TO_BOOKS);

            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;

            HttpClient client = new HttpClient(handler);

            var result = await client.GetAsync("https://www.gutenberg.org" + _toBeDownloaded.URL + ".epub3.images");

            if (result.StatusCode == HttpStatusCode.Redirect)
            {
                var location = result.Headers.Location;
                result = await client.GetAsync(location);
            }

            using (var fs = new FileStream(PATH_TO_BOOKS + _toBeDownloaded.Title + "." + Format.Epub.GetString(),
                FileMode.CreateNew))
            {
                fs.Position = 0;
                await result.Content.CopyToAsync(fs);
            }
        }
    }
}
