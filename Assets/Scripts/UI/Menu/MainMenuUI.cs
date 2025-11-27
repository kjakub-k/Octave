using KJakub.Octave.UI.Core;
using UnityEngine;
using UnityEngine.UIElements;
namespace KJakub.Octave.UI.Menu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private float scrollSpeed = 20f;
        [SerializeField]
        private UIController uiController;
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
            root.Q<Button>("EditorBtn").clicked += () =>
            {
                uiController.ShowEditor();
                uiController.ShowGame();
                uiController.HideMainMenu();
            };
            root.Q<Button>("ExitBtn").clicked += () =>
            {
                Application.Quit();
            };
        }
    }
}