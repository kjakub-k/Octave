using UnityEngine;
namespace KJakub.Octave.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TotalScoreAchievement", menuName = "Octave/Achievements/TotalScoreAchievement")]
    public class TotalScoreAchievementSO : AchievementSO
    {
        [SerializeField]
        private int score;
        public override bool IsUnlocked()
        {
            if (PlayerPrefs.GetInt("TotalScore", 0) >= score)
                return true;
            else
                return false;
        }
    }
}