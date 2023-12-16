using Newtonsoft.Json;
using System.Collections.Generic;
using Szakdolgozat.Models;

namespace Szakdolgozat.Services.Extensions
{
    public class DictionaryJsonExtensions : SerializeJsonBase
    {
        public DictionaryJsonExtensions()
        {
            Target = "C:\\Users\\Bence\\Documents\\Reader\\Jsons\\Dictionary.json";
        }

        public void SerializeWordToDictionary(string hungarianText, string englishText)
        {
            var dictJson = new DictionaryDataJson(hungarianText, englishText);

            Serialize(dictJson);
        }

        public DictionaryDataJson[] DeserializeJson()
        {
            return Deserialize<DictionaryDataJson>();
        }

    }
}
