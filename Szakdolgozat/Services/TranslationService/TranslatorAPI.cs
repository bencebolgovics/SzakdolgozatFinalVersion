using DeepL;
using System.Linq;
using System.Threading.Tasks;
using Szakdolgozat.Models;
using Szakdolgozat.Services.Extensions;

namespace Szakdolgozat.Services.TranslationServices
{
    public class TranslatorAPI
    {
        private const string _authorkey = "62631f46-2edb-8bfb-c35e-9f99e2efb701:fx";
        private Translator _translator; 
        private readonly DictionaryJsonExtensions _serializer;

        public TranslatorAPI() 
        {
            _translator = new Translator(_authorkey);
            _serializer = new DictionaryJsonExtensions();
        }

        public async Task<string> TranslateAndSaveToDictionary(string textToTranslate)
        {
            var wordFromDictionary = GetFromDictionary(textToTranslate);

            if (wordFromDictionary != null)
            {
                return wordFromDictionary.HungarianText;
            }

            //Here null means DeepL will detect the language
            var translatedText = await _translator.TranslateTextAsync(textToTranslate,
                                        null,
                                        LanguageCode.Hungarian);

            var translatedTextString = translatedText.ToString();

            SaveToDictionary(translatedTextString, textToTranslate);

            return translatedTextString;
        }

        private DictionaryDataJson? GetFromDictionary(string text)
        {
            DictionaryDataJson[] dict = _serializer.DeserializeJson();

            var wordInDictionary = dict.FirstOrDefault(x => x.ForeignText == text.ToLower());

            return wordInDictionary;
        }

        private void SaveToDictionary(string translatedText, string englishText)
        {
            _serializer.SerializeWordToDictionary(translatedText.ToLower(), englishText.ToLower());

        }

    }
}
