using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
namespace KJakub.Octave.Managers.LanguageManager
{
    public static class LanguageManager
    {
        private static Dictionary<string, string> translations;
        private const string PathToLanguages = "Assets/External/Languages/";
        public static string GetTranslation(string key)
        {
            if (translations[key] == "" || translations[key] == null)
            {
                return "NO TRANSLATION";
            } else
            {
                return translations[key];
            }
        }
        public static void SetLanguage(string language)
        {
            string json = File.ReadAllText($"{PathToLanguages}{language}.json");
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            translations = dict;
        }
    }
}