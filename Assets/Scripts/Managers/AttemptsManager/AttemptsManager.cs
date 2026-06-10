using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace KJakub.Octave.Managers.AttemptsManager
{
    public class AttemptsManager : MonoBehaviour
    {
        private static AttemptsManager instance;
        public static AttemptsManager Instance { get { return instance; } }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void Save(string levelID, int score)
        {
            string path = GetPath(levelID);
            string json = File.ReadAllText(path);

            List<int> scores = JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>();
            scores.Add(score);

            json = JsonConvert.SerializeObject(scores);
            File.WriteAllText(path, json);
        }
        public List<int> Get(string levelID, int maxAmount)
        {
            string path = GetPath(levelID);
            string json = File.ReadAllText(path);

            List<int> scores = JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>();

            scores = scores
                        .OrderByDescending(s => s)
                        .Take(maxAmount)
                        .ToList();

            return scores;
        }
        private string GetPath(string levelID)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, "AttemptsPlayerData");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{levelID}.json");

            if (!File.Exists(filePath))
            {
                List<int> scoresPlaceholder = new();
                string json = JsonConvert.SerializeObject(scoresPlaceholder);
                File.WriteAllText(filePath, json);
            }

            return filePath;
        }
    }
}