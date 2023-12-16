using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models.Enum
{
    public enum Format
    {
        None = 0,
        Epub = 1,
        Mobi = 2,
    }

    public static class FormatsExtensions
    {
        public static string GetString(this Format format)
        {
            switch (format)
            {
                case Format.None:
                    return string.Empty;
                case Format.Epub: 
                    return "epub";
                case Format.Mobi:
                    return "mobi";
                default: 
                    return string.Empty;
            }
        }

        public static Format GetFormatEnum(this string ext)
        {
            switch (ext)
            {
                case "epub":
                    return Format.Epub;
                case "mobi":
                    return Format.Mobi;
                default:
                    return Format.None;
            }
        }
    }
}
