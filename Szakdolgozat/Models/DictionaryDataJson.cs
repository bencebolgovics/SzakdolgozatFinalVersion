using System;

namespace Szakdolgozat.Models
{
    public class DictionaryDataJson : IEquatable<DictionaryDataJson>
    {
        public string HungarianText;
        public string ForeignText;

        public DictionaryDataJson(string hungarianText, string foreignText)
        {
            HungarianText = hungarianText;
            ForeignText = foreignText;
        }

        bool IEquatable<DictionaryDataJson>.Equals(DictionaryDataJson? other)
        {
            if (other == null) return false;

            return HungarianText == other.HungarianText ||
                   ForeignText == other.ForeignText;
        }
    }
}
