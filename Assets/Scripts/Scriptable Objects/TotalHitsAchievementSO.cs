using UnityEngine;
namespace KJakub.Octave.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TotalHitsAchievement", menuName = "Octave/Achievements/TotalHitsAchievement")]
    public class TotalHitsAchievementSO : AchievementSO
    {
        [SerializeField]
        private int totalHits;
        public override bool IsUnlocked()
        {
            if (PlayerPrefs.GetInt("TotalHits", 0) >= totalHits)
                return true;
            else
                return false;
        }
    }
}