using UnityEngine;
namespace KJakub.Octave.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HighestComboAchievement", menuName = "Octave/Achievements/HighestComboAchievement")]
    public class HighestComboAchievementSO : AchievementSO
    {
        [SerializeField]
        private int highestCombo;
        public override bool IsUnlocked()
        {
            if (PlayerPrefs.GetInt("HighestCombo", 0) >= highestCombo)
                return true;
            else
                return false;
        }
    }
}
