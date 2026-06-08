using UnityEngine;

namespace KJakub.Octave.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AverageAccuracyAchievement", menuName = "Octave/Achievements/AverageAccuracyAchievement")]
    public class AverageAccuracyAchievementSO : AchievementSO
    {
        [SerializeField]
        private int avgAccuracy;
        public override bool IsUnlocked()
        {
            if (PlayerPrefs.GetInt("AverageAccuracy", 0) >= avgAccuracy)
                return true;
            else
                return false;
        }
    }
}