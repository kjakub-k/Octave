using UnityEngine;
using UnityEngine.UIElements;
using KJakub.Octave.Data;
using KJakub.Octave.Editor.Logic;
using KJakub.Octave.Managers.CommandManager.NoteCommandManager;
using KJakub.Octave.Game.Core;
using KJakub.Octave.UI.Core;
namespace KJakub.Octave.UI.Editor
{ 
    public class EditorUI : MonoBehaviour
    {
        private NoteCommandManager cmdManager = new();
        private SongData currentSongData = new();
        [Header("Managers")]
        [SerializeField]
        private GameController gameController;
        [SerializeField]
        private UIController uiController;
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            InitializeScripts(root);
        }
        private void InitializeScripts(VisualElement root)
        {
            EditorPopupUI popup = new(root);
            SaveUI saveWindow = new(root, new SaveLogic(), currentSongData);
            NavbarUI navbar = new(root, new NavbarLogic(cmdManager, currentSongData, popup, saveWindow, gameController, uiController));
            ReturnToEditorBtn(root);
            InfoUI info = new(root, currentSongData);
            TimelineUI timeline = new(root, currentSongData, cmdManager);
        }
        private void ReturnToEditorBtn(VisualElement root)
        {
            Button btn = root.Q<Button>("ReturnToEditorBtn");
            
            btn.clicked += () => {
                uiController.HideGame();
                root.Q<VisualElement>("Background").RemoveFromClassList("closed");
                root.Q<VisualElement>("RTEContainer").AddToClassList("closed");
                gameController.EndGame();
            };
        }
    }
}