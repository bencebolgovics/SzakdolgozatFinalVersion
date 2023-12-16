using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Szakdolgozat.Models;

namespace Szakdolgozat.Services.Scraping
{
    public class GutenbergEbookMetadataProvider
    {
        private readonly HttpClient _client;

        public GutenbergEbookMetadataProvider()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All
            };

            _client = new HttpClient(handler);
        }

        public async Task<BookMetadata> GetBookMetadata(string bookId)
        {
            var json = await GetJsonFromGutendex(bookId);

            var id = json.GetValue("results")?.First?.Value<string>("id") ?? string.Empty;
            var title = json.GetValue("results")?.First?.Value<string>("title") ?? string.Empty;

            return new BookMetadata(id, title);
        }

        private async Task<JObject> GetJsonFromGutendex(string bookId)
        {
            using HttpResponseMessage response = await _client.GetAsync("https://gutendex.com/books?ids=" + bookId);

            var responseString = await response.Content.ReadAsStringAsync();

            return JObject.Parse(responseString);
        }

        private async Task<JObject> GetTrendingBooks()
        {
            using HttpResponseMessage response = await _client.GetAsync("https://gutendex.com/books?popular");

            var responseString = await response.Content.ReadAsStringAsync();

            return JObject.Parse(responseString);
        }

        public async Task<List<int>> GetTop30Books()
        {
            var trending = await GetTrendingBooks();

            List<int> bookIds = new List<int>(30);

            var items = trending.GetValue("results")?.Values<JToken>();

            foreach (var child in items)
            {
                bookIds.Add(child.First.First.Value<int>());
            }

            return bookIds;
        }
    }
}
