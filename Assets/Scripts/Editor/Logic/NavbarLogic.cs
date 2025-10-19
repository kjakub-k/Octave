using KJakub.Octave.Data;
using KJakub.Octave.Managers.CommandManager;
using KJakub.Octave.Managers.AudioFileManager;
using SFB;
using System.Threading.Tasks;
using UnityEngine;
using KJakub.Octave.Editor.Interfaces;
namespace KJakub.Octave.Editor.Logic
{
    public class NavbarLogic
    {
        private ICommandManager cmdManager;
        private IAudioFileManager audioFileManager;
        private SongData songData;
        private IEditorPopup popup;
        public NavbarLogic(ICommandManager cmdManager, IAudioFileManager audioFileManager, SongData songData, IEditorPopup popup)
        {
            (this.cmdManager, this.audioFileManager, this.songData, this.popup) = (cmdManager, audioFileManager, songData, popup);
        }
        public void Save()
        {
            Debug.Log("Saved");
        }
        public void Load()
        {
            Debug.Log("Loaded");
        }
        public void Undo()
        {
            cmdManager.Undo();
        }
        public void Redo()
        {
            cmdManager.Redo();
        }
        public async Task SetSong()
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Select WAV File", "", "wav", false);
            
            if (paths.Length > 0)
            {
                var path = paths[0];
                songData.Song = await audioFileManager.LoadAudioClip(path);
            }
        }
        public async Task SetBPM()
        {
            int? bpm = await popup.RequestInt();

            if (bpm != null)
                songData.BPM = bpm.Value;
        }
        public async Task SetLineAmount()
        {
            int? lines = await popup.RequestInt();

            if (lines != null)
                songData.Lines = lines.Value;
        }
        public async Task SetSnapping()
        {
            SnappingType? snapping = await popup.RequestSnappingType();
            
            if (snapping != null)
                songData.Snapping = snapping.Value;
        }
    }
}