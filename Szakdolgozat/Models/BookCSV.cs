namespace Szakdolgozat.Models
{
    public class BookMetadata
    {
        public string BookCode { get; set; }
        public string Title { get; set; }

        public BookMetadata(string bookCode, string title)
        {
            BookCode = bookCode;
            Title = title;
        }
    }
}
