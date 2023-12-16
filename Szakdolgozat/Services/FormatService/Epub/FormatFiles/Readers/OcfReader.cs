using System;
using System.Xml.Linq;
using Szakdolgozat.FormatServices.Epub.Extensions;

namespace Szakdolgozat.FormatServices.Epub.Format.Readers
{
    internal static class OcfReader
    {
        public static OcfDocument Read(XDocument xml)
        {
            var rootFiles = xml.Root?.Element(OcfElements.RootFiles)?.Elements(OcfElements.RootFile);
            var rootDoc = new OcfDocument
            {
                RootFiles = rootFiles.AsObjectList(elem => new OcfRootFile
                {
                    FullPath = (string) elem.Attribute(OcfRootFile.Attributes.FullPath),
                    MediaType = (string) elem.Attribute(OcfRootFile.Attributes.MediaType)
                })
            };
            return rootDoc;
        }
    }
}
