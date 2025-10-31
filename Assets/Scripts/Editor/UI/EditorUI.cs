using UnityEngine;
using UnityEngine.UIElements;
using KJakub.Octave.Data;
using KJakub.Octave.Editor.Logic;
using KJakub.Octave.Managers.CommandManager.NoteCommandManager;
namespace KJakub.Octave.Editor.UI 
{ 
    public class EditorUI : MonoBehaviour
    {
        [SerializeField]
        private VisualTreeAsset editorLayout;
        private NoteCommandManager cmdManager = new();
        private SongData currentSongData = new();
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
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
            NavbarUI navbar = new(root, new NavbarLogic(cmdManager, currentSongData, popup, saveWindow));
            InfoUI info = new(root, currentSongData);
            TimelineUI timeline = new(root, currentSongData, cmdManager);
        }
    }
}