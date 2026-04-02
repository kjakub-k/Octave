using KJakub.Octave.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Plastic.Newtonsoft.Json;
using System.IO;
namespace KJakub.Octave.Managers.AchievementsManager
{
    public class AchievementsManager : MonoBehaviour
    {
        private List<string> achievementIds = new();
        [SerializeField]
        private List<AchievementSO> achievements;
        public string Path { get { return $"{Folder}achievements.json"; } }
        public string Folder { get { return $"{Application.persistentDataPath}/PlayerData/"; } }
        private void Start()
        {
            achievementIds = GetUnlockedAchievementsIds();

            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            if (!File.Exists(Path))
                CreateAchievementsFile();
        }
        public AchievementSO ReturnAchievementByID(string id)
        {
            return achievements.Where(a => a.ID == id).FirstOrDefault();
        }
        public AchievementSO[] GetLockedAchievements()
        {
            return achievements.Where(a => !achievementIds.Contains(a.ID)).ToArray();
        }
        private void UnlockAchievement(AchievementSO achievement)
        {
            Debug.Log($"{achievement.ID} has been unlocked");
        }
        public List<string> GetUnlockedAchievementsIds()
        {
            if (!File.Exists(Path))
                CreateAchievementsFile();

            string json = File.ReadAllText(Path);
            List<string> result = JsonConvert.DeserializeObject<List<string>>(json);

            return result;
        }
        public AchievementSO[] GetUnlockedAchievements()
        {
            List<string> resultStr = GetUnlockedAchievementsIds();
            AchievementSO[] result = new AchievementSO[resultStr.Count];

            for (int i = 0; i < resultStr.Count; i++)
            {
                result[i] = ReturnAchievementByID(resultStr[i]);
            }

            return result;
        }
        private void CreateAchievementsFile()
        {
            List<string> fakeList = new List<string>();
            string json = JsonConvert.SerializeObject(fakeList);
            File.WriteAllText(Path, json);
        }
        public void Save()
        {
            string json = JsonConvert.SerializeObject(achievementIds);
            File.WriteAllText(Path, json);
        }
        public void CheckEligibleAchievements()
        {
            var lockedAchievements = GetLockedAchievements();

            for (int i = 0; i < lockedAchievements.Length; i++)
            {
                if (lockedAchievements[i].IsUnlocked())
                    UnlockAchievement(lockedAchievements[i]);
            }
        }
    }
}
