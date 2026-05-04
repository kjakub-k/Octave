using KJakub.Octave.Game.Core;
using KJakub.Octave.Managers.LanguageManager;
using KJakub.Octave.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Practice 
{
    public class PracticeModeUI : MonoBehaviour
    {
        [SerializeField]
        private PracticeController practiceController;
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private Slider startSlider;
        [SerializeField]
        private Slider endSlider;
        [SerializeField]
        private Slider timeSlider;
        [SerializeField]
        private TMP_Text timeLabel;
        [Header("Labels (For Translation)")]
        [SerializeField]
        private TMP_Text startLabel;
        [SerializeField]
        private TMP_Text endLabel;
        [SerializeField]
        private TMP_Text speedLabel;
        [SerializeField]
        private TMP_Text exitBtnLabel;
        private bool isDragging;
        private void OnEnable()
        {
            Translate();
        }
        private void Translate()
        {
            startLabel.text = LanguageManager.GetTranslation("start");
            endLabel.text = LanguageManager.GetTranslation("end");
            speedLabel.text = LanguageManager.GetTranslation("speed");
            exitBtnLabel.text = LanguageManager.GetTranslation("exit_btn");
        }
        public void OnTimelinePointerDown()
        {
            isDragging = true;
        }
        public void OnTimelinePointerUp()
        {
            isDragging = false; 
        }

        private void Update()
        {
            if (!isDragging)
            {
                timeSlider.value = practiceController.Timer;
            }

            timeLabel.text = FormatTime(practiceController.Timer);
        }
        public void OnTimelineChanged(float value)
        {
            if (isDragging)
                practiceController.SpawnNotes(value);
        }
        private void Awake()
        {
            practiceController.OnPracticeModeEntered += UpdateSliders;
        }
        private void UpdateSliders()
        {
            startSlider.maxValue = practiceController.SongData.Song.length;
            startSlider.value = 0;
            endSlider.maxValue = practiceController.SongData.Song.length;
            endSlider.value = practiceController.SongData.Song.length;
            timeSlider.value = 0;
            timeSlider.maxValue = practiceController.SongData.Song.length;
        }
        public static string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

            return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
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