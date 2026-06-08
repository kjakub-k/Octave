using UnityEngine;
namespace KJakub.Octave.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TotalMissesAchievement", menuName = "Octave/Achievements/TotalMissesAchievement")]
    public class TotalMissesAchievementSO : AchievementSO
    {
        [SerializeField]
        private int totalMisses;
        public override bool IsUnlocked()
        {
            if (PlayerPrefs.GetInt("TotalMisses", 0) >= totalMisses)
                return true;
            else
                return false;
        }
    }
}