using UnityEngine;
using UnityEngine.UIElements;
namespace KJakub.Octave.UI.Core
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private UIDocument editorLayout;
        [SerializeField]
        private Canvas gameLayout;
        [SerializeField]
        private Canvas albumSelectionLayout;
        [SerializeField]
        private Canvas levelSelectionLayout;
        [SerializeField]
        private UIDocument mainMenuLayout;
        private void Awake()
        {
            editorLayout.gameObject.SetActive(true);
            mainMenuLayout.gameObject.SetActive(true);
            levelSelectionLayout.gameObject.SetActive(true);
            albumSelectionLayout.gameObject.SetActive(true);
            gameLayout.gameObject.SetActive(true);

            ShowMainMenu();
            HideGame();
            HideEditor();
            HideLevelSelectionMenu();
            HideAlbumSelectionMenu();
        }
        public void ShowMainMenu()
        {
            mainMenuLayout.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        public void HideMainMenu()
        {
            mainMenuLayout.rootVisualElement.style.display = DisplayStyle.None;
        }
        public void ShowEditor()
        {
            editorLayout.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        public void HideEditor()
        {
            editorLayout.rootVisualElement.style.display = DisplayStyle.None;
        }
        public void ShowGame()
        {
            gameLayout.gameObject.SetActive(true);
        }
        public void HideGame()
        {
            gameLayout.gameObject.SetActive(false);
        }
        public void ShowAlbumSelectionMenu()
        {
            albumSelectionLayout.gameObject.SetActive(true);
        }
        public void HideAlbumSelectionMenu()
        {
            albumSelectionLayout.gameObject.SetActive(false);
        }
        public void ShowLevelSelectionMenu()
        {
            levelSelectionLayout.gameObject.SetActive(true);
        }
        public void HideLevelSelectionMenu()
        {
            levelSelectionLayout.gameObject.SetActive(false);
        }
    }
}