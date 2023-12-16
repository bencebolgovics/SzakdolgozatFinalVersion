using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Szakdolgozat.FormatServices.Epub.Extensions;
using Szakdolgozat.Models.Enum;
using Szakdolgozat.Services.FormatService.Epub.FormatFiles;

namespace Szakdolgozat.FormatServices.Epub.Format.Readers
{
    internal static class OpfReader
    {
        public static OpfDocument Read(XDocument xml)
        {
            Func<XElement, OpfMetadataCreator> readCreator = elem => new OpfMetadataCreator
            {
                Text = elem.Value
            };

            var epubVersion = GetAndValidateVersion((string) xml.Root.Attribute(OpfDocument.Attributes.Version));
            var metadata = xml.Root.Element(OpfElements.Metadata);
            var spine = xml.Root.Element(OpfElements.Spine);

            var opfDocument = new OpfDocument
            {
                Metadata = new OpfMetadata
                {
                    Creators = metadata?.Elements(OpfElements.Creator).AsStringList(),
                    Metas = metadata?.Elements(OpfElements.Meta).AsObjectList(elem => new OpfMetadataMeta
                    {
                        Name = (string) elem.Attribute(OpfMetadataMeta.Attributes.Name),
                        Id = epubVersion == EpubVersion.Epub2 ? (string) elem.Attribute(OpfMetadataMeta.Attributes.Content) : elem.Value
                    }),
                    Titles = metadata?.Elements(OpfElements.Title).AsStringList(),
                },
                Manifest = new OpfManifest
                {
                    Items = xml.Root.Element(OpfElements.Manifest)?.Elements(OpfElements.Item).AsObjectList(elem => new OpfManifestItem
                    {
                        Href = (string) elem.Attribute(OpfManifestItem.Attributes.Href),
                        Id = (string) elem.Attribute(OpfManifestItem.Attributes.Id),
                        MediaType = (string) elem.Attribute(OpfManifestItem.Attributes.MediaType),
                        Properties = ((string) elem.Attribute(OpfManifestItem.Attributes.Properties))?.Split(' ') ?? new string[0],
                    })
                },
                Spine = new OpfSpine
                {
                    ItemRefs = spine?.Elements(OpfElements.ItemRef).AsObjectList(elem => new OpfSpineItemRef
                    {
                        IdRef = (string) elem.Attribute(OpfSpineItemRef.Attributes.IdRef),
                    }),
                }
            };

            return opfDocument;
        }

        private static EpubVersion GetAndValidateVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version)) throw new ArgumentNullException(nameof(version));

            if (version == "2.0")
            {
                return EpubVersion.Epub2;
            }
            if (version == "3.0" || version == "3.0.1" || version == "3.1")
            {
                return EpubVersion.Epub3;
            }

            throw new Exception($"Unsupported EPUB version: {version}.");
        }
    }
}
