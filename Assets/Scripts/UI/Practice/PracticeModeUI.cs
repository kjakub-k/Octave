using KJakub.Octave.Game.Core;
using KJakub.Octave.UI.Core;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
namespace KJakub.Octave.UI.Practice 
{
    public class PracticeModeUI : MonoBehaviour
    {
        [SerializeField]
        private PracticeController practiceController;
        [SerializeField]
        private UIController uiController;
        public void StepForward()
        {
            practiceController.ChangeTime(5);
        }
        public void StepBackward()
        {
            practiceController.ChangeTime(-5);
        }
        public void OnStartSliderChanged(float value)
        {
            practiceController.ChangeStartLoop(value);
        }
        public void OnEndSliderChanged(float value)
        {
            practiceController.ChangeEndLoop(value);
        }
        public void OnSpeedSliderChanged(float value)
        {
            practiceController.ChangeSpeed(value);
        }
        public void BackToMenu()
        {
            practiceController.Stop();
            uiController.ShowLevelSelectionMenu();
            uiController.HidePracticeMode();
        }
    }
}