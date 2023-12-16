using System.Collections.Generic;
using System.Linq;
using System.Text;
using Szakdolgozat.FormatServices.Epub.Format;
using Szakdolgozat.Models.Constants;
using Szakdolgozat.Models.Enum;
using Szakdolgozat.Services.FormatService.Epub;

namespace Szakdolgozat.Services.FormatServices.Epub
{
    public class Epub
    {
        public EpubFormat Format { get; set; }

        public string Title => Format.Opf.Metadata.Titles.FirstOrDefault();

        public IEnumerable<string> Authors => Format.Opf.Metadata.Creators.Select(creator => creator);

        public EpubResources Resources { get; internal set; }

        public EpubSpecialResources SpecialResources { get; internal set; }

        public byte[] CoverImage { get; internal set; }

        public string ToText()
        {
            var builder = new StringBuilder();
            foreach (var html in SpecialResources.HtmlInReadingOrder)
            {
                builder.Append(Html.GetContentAsPlainText(html.TextContent));
                builder.Append('\n');
            }
            return builder.ToString().Trim();
        }
    }

    public class EpubResources
    {
        public IList<EpubTextFile> Html { get; internal set; } = new List<EpubTextFile>();
        public IList<EpubByteFile> Images { get; internal set; } = new List<EpubByteFile>();
    }

    public class EpubSpecialResources
    {
        public IList<EpubTextFile> HtmlInReadingOrder { get; internal set; } = new List<EpubTextFile>();
    }

    public abstract class EpubFile
    {
        public string AbsolutePath { get; set; }
        public string Href { get; set; }
        public EpubContentType ContentType { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }
    }

    public class EpubByteFile : EpubFile
    {
    }

    public class EpubTextFile : EpubFile
    {
        public string TextContent
        {
            get { return Constants.DefaultEncoding.GetString(Content, 0, Content.Length); }
            set { Content = Constants.DefaultEncoding.GetBytes(value); }
        }
    }
}
