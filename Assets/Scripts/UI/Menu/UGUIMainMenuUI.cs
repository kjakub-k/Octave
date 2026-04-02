using KJakub.Octave.Managers.JsonManager;
using KJakub.Octave.UI.AlbumSelect;
using KJakub.Octave.UI.Core;
using UnityEngine;
namespace KJakub.Octave.UI.Menu
{
    public class UGUIMainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private AlbumSelectUI albumSelectUI;
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