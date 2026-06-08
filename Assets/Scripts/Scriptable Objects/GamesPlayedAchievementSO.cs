using UnityEngine;
namespace KJakub.Octave.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GamesPlayedAchievement", menuName = "Octave/Achievements/GamesPlayedAchievement")]
    public class GamesPlayedAchievementSO : AchievementSO
    {
        [SerializeField]
        private int gamesPlayed;
        public override bool IsUnlocked()
        {
            if (PlayerPrefs.GetInt("GamesPlayed", 0) >= gamesPlayed)
                return true;
            else
                return false;
        }
    }
}