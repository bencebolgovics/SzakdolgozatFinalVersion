using HtmlAgilityPack;
using System.Collections.Generic;
using Szakdolgozat.Models;

namespace Szakdolgozat.Services.Scraping
{
    public static class SearchResultProvider
    {
        private const string NODE_SELECTOR = ".//li[@class='booklink']/a/span[@class='cell content']";
        private const string TITLE_SELECTOR = ".//span[@class='title']";
        private const string AUTHOR_SELECTOR = ".//span[@class='subtitle']";
        private const string EXTRA_SELECTOR = ".//span[@class='extra']";
        private const string SUBMIT_SEARCH_SELECTOR = "&submit_search=Search";
        private const string ANONIM_AUTHOR = "anonim";

        public static IEnumerable<SearchResultDTO> GetSearchResult(string searchText)
        {
            string inputStringText = searchText.Replace(' ', '+');

            HtmlWeb web = new HtmlWeb();
            var doc = web.Load("https://www.gutenberg.org/ebooks/search/?query="+ inputStringText + SUBMIT_SEARCH_SELECTOR);

            var nodes = doc.DocumentNode.SelectNodes(NODE_SELECTOR);

            return CreateSearchResult(nodes);
        }

        private static IEnumerable<SearchResultDTO> CreateSearchResult(HtmlNodeCollection? nodes)
        {
            if (nodes== null || nodes.Count == 0) 
                yield break;

            foreach(var node in nodes)
            {
                string title = node.SelectSingleNode(TITLE_SELECTOR).InnerText;
                string author = node.SelectSingleNode(AUTHOR_SELECTOR)?.InnerText ?? ANONIM_AUTHOR;
                string extra = node.SelectSingleNode(EXTRA_SELECTOR).InnerText;
                string url = node.ParentNode.Attributes["href"].Value;

                yield return new SearchResultDTO(title, author, extra, url);
            }
        }
    }
}
