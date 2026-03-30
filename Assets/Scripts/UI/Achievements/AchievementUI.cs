using KJakub.Octave.UI.Core;
using UnityEngine;
namespace KJakub.Octave.UI.Achievements
{
    public class AchievementUI : MonoBehaviour
    {
        [SerializeField]
        private UIController uiController;
        private void OnEnable()
        {
            
        }
        public void BackToMenu()
        {
            uiController.ShowMainMenu();
            uiController.HideAchievements();
        }
    }
}