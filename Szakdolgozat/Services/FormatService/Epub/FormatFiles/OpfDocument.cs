using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Szakdolgozat.Services.FormatService.Epub.FormatFiles
{
    public class OpfDocument
    {
        internal static class Attributes
        {
            public static readonly XName Version = "version";
        }

        public OpfMetadata Metadata { get; internal set; } = new OpfMetadata();
        public OpfManifest Manifest { get; internal set; } = new OpfManifest();
        public OpfSpine Spine { get; internal set; } = new OpfSpine();

        internal string FindCoverPath()
        {
            var coverMetaItem = Metadata.FindCoverMeta();
            if (coverMetaItem != null)
            {
                var item = Manifest.Items.FirstOrDefault(e => e.Id == coverMetaItem.Id);
                if (item != null)
                {
                    return item.Href;
                }
            }

            var coverItem = Manifest.FindCoverItem();
            return coverItem?.Href;
        }
    }

    public class OpfMetadata
    {
        public IList<string> Titles { get; internal set; } = new List<string>();
        public IList<string> Creators { get; internal set; } = new List<string>();
        public IList<OpfMetadataMeta> Metas { get; internal set; } = new List<OpfMetadataMeta>();

        internal OpfMetadataMeta FindCoverMeta()
        {
            return Metas.FirstOrDefault(metaItem => metaItem.Name == "cover");
        }
    }

    public class OpfMetadataCreator
    {
        public string Text { get; internal set; }
    }

    public class OpfMetadataMeta
    {
        internal static class Attributes
        {
            public static readonly XName Id = "id";
            public static readonly XName Name = "name";
            public static readonly XName Refines = "refines";
            public static readonly XName Scheme = "scheme";
            public static readonly XName Property = "property";
            public static readonly XName Content = "content";
        }

        public string Name { get; internal set; }
        public string Id { get; internal set; }
    }

    public class OpfManifest
    {
        internal const string ManifestItemCoverImageProperty = "cover-image";

        public IList<OpfManifestItem> Items { get; internal set; } = new List<OpfManifestItem>();

        internal OpfManifestItem FindCoverItem()
        {
            return Items.FirstOrDefault(e => e.Properties.Contains(ManifestItemCoverImageProperty));
        }
    }

    public class OpfManifestItem
    {
        internal static class Attributes
        {
            public static readonly XName Fallback = "fallback";
            public static readonly XName FallbackStyle = "fallback-style";
            public static readonly XName Href = "href";
            public static readonly XName Id = "id";
            public static readonly XName MediaType = "media-type";
            public static readonly XName Properties = "properties";
            public static readonly XName RequiredModules = "required-modules";
            public static readonly XName RequiredNamespace = "required-namespace";
        }

        public string Id { get; internal set; }
        public string Href { get; internal set; }
        public IList<string> Properties { get; internal set; } = new List<string>();
        public string MediaType { get; internal set; }
    }

    public class OpfSpine
    {
        public IList<OpfSpineItemRef> ItemRefs { get; internal set; } = new List<OpfSpineItemRef>();
    }

    public class OpfSpineItemRef
    {
        internal static class Attributes
        {
            public static readonly XName IdRef = "idref";
            public static readonly XName Linear = "linear";
            public static readonly XName Id = "id";
            public static readonly XName Properties = "properties";
        }

        public string IdRef { get; internal set; }
    }
}
