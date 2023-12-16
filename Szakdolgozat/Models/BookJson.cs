namespace Szakdolgozat.Models
{
    public class BookJson
    {
        public int BookCode { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }
        public string CoverId { get; set; }

        public BookJson(int bookCode, string title, string filePath, string author, int pageCount, string coverId)
        {
            BookCode = bookCode;
            Title = title;
            Author = author;
            PageCount = pageCount;
            CoverId = coverId;
            FilePath = filePath;
        }
    }
}
