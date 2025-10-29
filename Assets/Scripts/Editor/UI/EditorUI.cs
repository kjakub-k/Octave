using UnityEngine;
using UnityEngine.UIElements;
using KJakub.Octave.Managers.CommandManager;
using KJakub.Octave.Managers.AudioFileManager;
using KJakub.Octave.Data;
using KJakub.Octave.Editor.Logic;
namespace KJakub.Octave.Editor.UI 
{ 
    public class EditorUI : MonoBehaviour
    {
        [SerializeField]
        private VisualTreeAsset editorLayout;
        private ICommandManager cmdManager = new CommandManager();
        private IAudioFileManager audioFileManager = new AudioFileManager();
        private SongData currentSongData = new();
        private NavbarUI navbar;
        private TimelineUI timeline;
        private EditorPopupUI popup;
        private SaveUI saveWindow;
        private InfoUI info;
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
            popup = new(root);
            saveWindow = new(root, new SaveLogic(audioFileManager), currentSongData);
            navbar = new(root, new NavbarLogic(cmdManager, audioFileManager, currentSongData, popup, saveWindow));
            info = new(root, currentSongData);
            timeline = new(root, currentSongData, cmdManager);
        }
    }
}