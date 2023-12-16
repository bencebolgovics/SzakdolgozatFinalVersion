using Szakdolgozat.FormatServices.Epub.Format;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Szakdolgozat.Services.FormatServices.Epub;
using Szakdolgozat.Models.Constants;

namespace Szakdolgozat.FormatServices.Epub.Extensions
{
    public static class ZipArchiveExtensions
    {   
        public static ZipArchiveEntry GetEntryImproved(this ZipArchive archive, string entryName)
        {
            var entry = GetEntryByTries(archive, entryName);
            if (entry == null)
            {
                throw new Exception("file not found");
            }
            return entry;
        }

        public static ZipArchiveEntry GetEntryByTries(this ZipArchive archive, string entryName)
        {
            if (entryName.StartsWith("/") || entryName.StartsWith("\\"))
            {
                entryName = entryName.Substring(1);
            }

            var entry = archive.GetEntry(entryName);

            if (entry == null)
            {
                var namesToTry = new List<string>();

                namesToTry.Add("/" + entryName);
                namesToTry.Add("\\" + entryName);

                namesToTry.Add(Uri.UnescapeDataString(entryName));

                foreach (var newName in namesToTry)
                {
                    entry = archive.GetEntry(newName);
                    if (entry != null)
                    {
                        break;
                    }
                }
            }

            return entry;
        }

        public static byte[] LoadBytes(this ZipArchive archive, string entryName)
        {
            var entry = archive.GetEntry(entryName);
            using (var stream = entry.Open())
            {
                var data = stream.ReadToEnd();
                return data;
            }
        }

        public static XDocument LoadXml(this ZipArchive archive, string entryName)
        {
            var entry = archive.GetEntryByTries(entryName);

            using (var stream = entry.Open())
            {
                var xml = LoadXDocumentWithoutDtd(stream);
                return xml;
            }
        }

        private static XDocument LoadXDocumentWithoutDtd(Stream stream)
        {
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
            using (var reader = XmlReader.Create(stream, settings))
                return XDocument.Load(reader);
        }
    }
}
