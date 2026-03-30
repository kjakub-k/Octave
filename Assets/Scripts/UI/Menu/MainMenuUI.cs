using KJakub.Octave.UI.AlbumSelect;
using KJakub.Octave.UI.Core;
using UnityEngine;
using UnityEngine.UIElements;
using KJakub.Octave.Managers.JsonManager;
namespace KJakub.Octave.UI.Menu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private float scrollSpeed = 20f;
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private AlbumSelectUI albumSelectUI;
        private ScrollingBackgroundUI bg;
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            InitializeScripts(root);
            InitializeElements(root);
        }
        private void InitializeScripts(VisualElement root)
        {
            bg = new(scrollSpeed, root.Q<VisualElement>("Background"));
        }
        private void InitializeElements(VisualElement root)
        {
            root.Q<Button>("PlayBtn").clicked += () =>
            {
                uiController.Transition(() =>
                {
                    uiController.ShowAlbumSelectionMenu();
                    albumSelectUI.Initialize(JsonAlbumAndLevelsLoader.LoadAllAlbums());
                    uiController.HideMainMenu();
                });
            };
            root.Q<Button>("EditorBtn").clicked += () =>
            {
                uiController.Transition(() =>
                {
                    uiController.ShowEditor();
                    uiController.ShowGame();
                    uiController.HideMainMenu();
                });
            };
            root.Q<Button>("SettingsBtn").clicked += () =>
            {
                uiController.Transition(() =>
                {
                    uiController.ShowSettings();
                    uiController.HideMainMenu();
                });
            };
            root.Q<Button>("ExitBtn").clicked += () =>
            {
                Application.Quit();
            };
        }
    }
}