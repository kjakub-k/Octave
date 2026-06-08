using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
namespace KJakub.Octave.Managers.LanguageManager
{
    public static class LanguageManager
    {
        private static Dictionary<string, string> translations;
        private static string PathToLanguages { get { return $"{Application.streamingAssetsPath}/Languages/"; } }
        public static string GetTranslation(string key)
        {
            if (!translations.ContainsKey(key))
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