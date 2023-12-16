﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Szakdolgozat.Models.Constants;

namespace Szakdolgozat.FormatServices.Epub.Format
{
    public class OcfDocument
    {
        public IList<OcfRootFile> RootFiles { get; internal set; } = new List<OcfRootFile>();

        public string RootFilePath => RootFiles.FirstOrDefault(e => e.MediaType == Constants.OcfMediaType)?.FullPath;
    }

    public class OcfRootFile
    {
        internal static class Attributes
        {
            public static readonly XName FullPath = "full-path";
            public static readonly XName MediaType = "media-type";
        }

        public string FullPath { get; internal set; }
        public string MediaType { get; internal set; }
    }
}
