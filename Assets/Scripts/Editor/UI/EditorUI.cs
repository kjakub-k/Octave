using UnityEngine;
using UnityEngine.UIElements;
using KJakub.Octave.Data;
using KJakub.Octave.Editor.Logic;
using KJakub.Octave.Managers.CommandManager.NoteCommandManager;
using KJakub.Octave.Game.Interfaces;
namespace KJakub.Octave.Editor.UI 
{ 
    public class EditorUI : MonoBehaviour
    {
        [SerializeField]
        private VisualTreeAsset editorLayout;
        private NoteCommandManager cmdManager = new();
        private SongData currentSongData = new();
        [SerializeField]
        private GameObject gameControllerGO;
        private IGameController gameController;
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            gameController = gameControllerGO.GetComponent<IGameController>();
            SwitchTo(root);
        }
        /// <summary>
        /// Changes the root to the editor's UI hierarchy.
        /// </summary>
        public void SwitchTo(VisualElement root)
        {
            root.Clear();
            editorLayout.CloneTree(root);
            EditorPopupUI popup = new(root);
            SaveUI saveWindow = new(root, new SaveLogic(), currentSongData);
            NavbarUI navbar = new(root, new NavbarLogic(cmdManager, currentSongData, popup, saveWindow, gameController));
            ReturnToEditorBtn(root);
            InfoUI info = new(root, currentSongData);
            TimelineUI timeline = new(root, currentSongData, cmdManager);
        }
        private void ReturnToEditorBtn(VisualElement root)
        {
            Button btn = root.Q<Button>("ReturnToEditorBtn");
            
            btn.clicked += () => {
                root.Q<VisualElement>("Background").RemoveFromClassList("closed");
                root.Q<VisualElement>("RTEContainer").AddToClassList("closed");
                gameController.EndGame();
            };
        }
    }
}