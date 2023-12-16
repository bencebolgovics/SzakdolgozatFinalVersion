namespace Szakdolgozat.Models
{
    public class SearchResultDTO
    {
        public string Title;
        public string Author;
        public string Extra;
        public string URL;

        public SearchResultDTO(string title, string author, string extra, string url)
        {
            Title = title;
            Author = author;
            Extra = extra;
            URL = url;
        }

        public override string ToString()
        {
            return Title + "\r\n"
               + Author + "\r\n"
               + Extra;
        }
    }
}
