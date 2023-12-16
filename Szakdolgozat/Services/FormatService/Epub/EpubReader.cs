using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Szakdolgozat.FormatServices.Epub.Extensions;
using Szakdolgozat.FormatServices.Epub.Format;
using Szakdolgozat.FormatServices.Epub.Format.Readers;
using Szakdolgozat.Models.Constants;
using Szakdolgozat.Models.Enum;

namespace Szakdolgozat.Services.FormatServices.Epub
{
    public static class EpubReader
    {
        public static Epub Read(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Specified epub file not found.", filePath);
            }

            return Read(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public static Epub Read(Stream stream)
        {

            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, false, Constants.DefaultEncoding))
            {
                var format = new EpubFormat { Ocf = OcfReader.Read(archive.LoadXml(Constants.OcfPath)) };

                format.Paths.OcfAbsolutePath = Constants.OcfPath;

                format.Paths.OpfAbsolutePath = format.Ocf.RootFilePath;
                if (format.Paths.OpfAbsolutePath == null)
                {
                    throw new Exception("Missing root file");
                }

                format.Opf = OpfReader.Read(archive.LoadXml(format.Paths.OpfAbsolutePath));

                var book = new Epub { Format = format };
                book.Resources = LoadResources(archive, book);
                book.SpecialResources = LoadSpecialResources(archive, book);
                book.CoverImage = LoadCoverImage(book);
                return book;
            }
        }

        private static byte[] LoadCoverImage(Epub book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            if (book.Format == null) throw new ArgumentNullException(nameof(book.Format));

            var coverPath = book.Format.Opf.FindCoverPath();
            if (coverPath == null)
            {
                return null;
            }

            var coverImageFile = book.Resources.Images.SingleOrDefault(e => e.Href == coverPath);
            return coverImageFile?.Content;
        }

        private static EpubResources LoadResources(ZipArchive epubArchive, Epub book)
        {
            var resources = new EpubResources();

            foreach (var item in book.Format.Opf.Manifest.Items)
            {
                var path = item.Href.ToAbsolutePath(book.Format.Paths.OpfAbsolutePath);
                var entry = epubArchive.GetEntryImproved(path);

                if (entry == null)
                {
                    throw new Exception("file not found");
                }

                var href = item.Href;
                var mimeType = item.MediaType;

                EpubContentType contentType;
                contentType = EpubContentTypeExtensions.MimeTypeToContentType.TryGetValue(mimeType, out contentType)
                    ? contentType
                    : EpubContentType.Other;

                switch (contentType)
                {
                    case EpubContentType.Xhtml11:
                        {
                            var file = new EpubTextFile
                            {
                                AbsolutePath = path,
                                Href = href,
                                MimeType = mimeType,
                                ContentType = contentType
                            };

                            using (var stream = entry.Open())
                            {
                                file.Content = stream.ReadToEnd();
                            }

                            resources.Html.Add(file);
                            break;
                        }
                    default:
                        {
                            var file = new EpubByteFile
                            {
                                AbsolutePath = path,
                                Href = href,
                                MimeType = mimeType,
                                ContentType = contentType
                            };

                            using (var stream = entry.Open())
                            {
                                if (stream == null)
                                {
                                    throw new Exception("file not found");
                                }

                                using (var memoryStream = new MemoryStream((int)entry.Length))
                                {
                                    stream.CopyTo(memoryStream);
                                    file.Content = memoryStream.ToArray();
                                }
                            }

                            switch (contentType)
                            {
                                case EpubContentType.ImageGif:
                                case EpubContentType.ImageJpeg:
                                case EpubContentType.ImagePng:
                                case EpubContentType.ImageSvg:
                                    resources.Images.Add(file);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                }
            }

            return resources;
        }

        private static EpubSpecialResources LoadSpecialResources(ZipArchive epubArchive, Epub book)
        {
            var result = new EpubSpecialResources
            {
                HtmlInReadingOrder = new List<EpubTextFile>()
            };

            var htmlFiles = book.Format.Opf.Manifest.Items
                .Where(item => EpubContentTypeExtensions.MimeTypeToContentType.ContainsKey(item.MediaType) && EpubContentTypeExtensions.MimeTypeToContentType[item.MediaType] == EpubContentType.Xhtml11)
                .ToDictionary(item => item.Id, item => item.Href);

            foreach (var item in book.Format.Opf.Spine.ItemRefs)
            {
                if (!htmlFiles.TryGetValue(item.IdRef, out string href))
                {
                    continue;
                }

                var html = book.Resources.Html.SingleOrDefault(e => e.Href == href);
                if (html != null)
                {
                    result.HtmlInReadingOrder.Add(html);
                }
            }

            return result;
        }
    }
}
