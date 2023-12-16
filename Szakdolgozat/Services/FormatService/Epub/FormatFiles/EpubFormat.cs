using Szakdolgozat.Services.FormatService.Epub.FormatFiles;

namespace Szakdolgozat.FormatServices.Epub.Format
{
    public class EpubFormatPaths
    {
        public string OcfAbsolutePath { get; internal set; }
        public string OpfAbsolutePath { get; internal set; }
    }

    public class EpubFormat
    {
        public EpubFormatPaths Paths { get; internal set; } = new EpubFormatPaths();

        public OcfDocument Ocf { get; internal set; }
        public OpfDocument Opf { get; internal set; }
    }
}
