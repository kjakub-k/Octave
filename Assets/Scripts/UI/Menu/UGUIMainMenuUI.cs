using KJakub.Octave.Managers.JsonManager;
using KJakub.Octave.Managers.LanguageManager;
using KJakub.Octave.UI.AlbumSelect;
using KJakub.Octave.UI.Core;
using TMPro;
using UnityEngine;
namespace KJakub.Octave.UI.Menu
{
    public class UGUIMainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private AlbumSelectUI albumSelectUI;
        [Header("Labels (For Translation)")]
        [SerializeField]
        private TMP_Text playBtnLabel;
        [SerializeField]
        private TMP_Text settingsBtnLabel;
        [SerializeField]
        private TMP_Text profileBtnLabel;
        [SerializeField]
        private TMP_Text exitBtnLabel;
        [SerializeField]
        private TMP_Text tutorialBtnLabel;
        [SerializeField]
        private TMP_Text achievementsBtnLabel;
        [SerializeField]
        private TMP_Text creditsBtnLabel;
        [SerializeField]
        private TMP_Text editorBtnLabel;
        private void OnEnable()
        {
            Translate();
        }
        private void Translate()
        {
            playBtnLabel.text = LanguageManager.GetTranslation("play_btn");
            settingsBtnLabel.text = LanguageManager.GetTranslation("settings_btn");
            profileBtnLabel.text = LanguageManager.GetTranslation("profile_btn");
            exitBtnLabel.text = LanguageManager.GetTranslation("exit_btn");
            tutorialBtnLabel.text = LanguageManager.GetTranslation("tutorial_btn");
            achievementsBtnLabel.text = LanguageManager.GetTranslation("achievements_btn");
            creditsBtnLabel.text = LanguageManager.GetTranslation("credits_btn");
            editorBtnLabel.text = LanguageManager.GetTranslation("editor_btn");
        }
        public void PlayBtn()
        {
            uiController.Transition(() =>
            {
                uiController.ShowAlbumSelectionMenu();
                albumSelectUI.Initialize(JsonAlbumAndLevelsLoader.LoadAllAlbums());
                uiController.HideMainMenu();
            });
        }
        public void ProfileBtn()
        {
            uiController.Transition(() =>
            {
                uiController.ShowProfile();
                uiController.HideMainMenu();
            });
        }
        public void SettingsBtn()
        {
            uiController.Transition(() =>
            {
                uiController.ShowSettings();
                uiController.HideMainMenu();
            });
        }
        public void AchievementsBtn()
        {
            uiController.Transition(() =>
            {
                uiController.ShowAchievements();
                uiController.HideMainMenu();
            });
        }
        public void EditorBtn()
        {
            uiController.Transition(() =>
            {
                uiController.ShowEditor();
                uiController.ShowGame();
                uiController.HideMainMenu();
            });
        }
        public void ExitBtn()
        {
            Application.Quit();
        }
    }
}