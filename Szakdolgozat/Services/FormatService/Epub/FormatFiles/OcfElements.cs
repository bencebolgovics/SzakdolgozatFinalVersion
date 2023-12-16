using System.Xml.Linq;
using Szakdolgozat.Models.Constants;

namespace Szakdolgozat.FormatServices.Epub.Format
{
    internal static class OcfElements
    {
        public static readonly XName RootFile = Constants.OcfNamespace + "rootfile";
        public static readonly XName RootFiles = Constants.OcfNamespace + "rootfiles";
    }
}
