using System;
using Szakdolgozat.Models.Constants;
using Szakdolgozat.Models.Enum;
using Szakdolgozat.Services.Strategies;

namespace Szakdolgozat.Services.BookExtractorStrategies
{
    public class ExtractorContext
    {
        public static IBookManipulatorStrategy GetStrategy(Format ext)
        {
            switch (ext)
            {
                case Format.Epub:
                    return new EpubExtractorStrategy();
                case Format.Mobi:
                    return new MobiExtractorStrategy();
                default: 
                    throw new NotImplementedException();
            }
        }
    }
}
