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
        private List<string> achievementIds;
        [SerializeField]
        private AchievementSO[] achievements;
        public string Path { get { return $"{Application.persistentDataPath}/PlayerData/achievements.json"; } }
        public AchievementSO ReturnAchievementByID(string id)
        {
            return achievements.ToList().Where(a => a.ID == id).FirstOrDefault();
        }
        public AchievementSO[] GetLockedAchievements()
        {

            return achievements.ToList().Where(a => !achievementIds.Contains(a.ID)).ToArray();
        }
        private void UnlockAchievement(AchievementSO achievement)
        {
            Debug.Log($"{achievement.ID} has been unlocked");
        }
        public List<string> GetUnlockedAchievements()
        {
            if (!File.Exists(Path))
                CreateAchievementsFile();

            string json = File.ReadAllText(Path);
            List<string> result = JsonConvert.DeserializeObject<List<string>>(json);
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
