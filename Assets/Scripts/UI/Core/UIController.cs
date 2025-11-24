using UnityEngine;
using UnityEngine.UIElements;

namespace KJakub.Octave.UI.Core
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private UIDocument editorLayout;
        [SerializeField]
        private UIDocument gameLayout;
        private void Start()
        {
            ShowEditor();
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
            gameLayout.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        public void HideGame()
        {
            gameLayout.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}