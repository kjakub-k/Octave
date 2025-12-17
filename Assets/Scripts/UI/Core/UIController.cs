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
        private UIDocument mainMenuLayout;
        private void Awake()
        {
            editorLayout.gameObject.SetActive(true);
            mainMenuLayout.gameObject.SetActive(true);
            gameLayout.gameObject.SetActive(true);

            ShowMainMenu();
            HideGame();
            HideEditor();
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
    }
}