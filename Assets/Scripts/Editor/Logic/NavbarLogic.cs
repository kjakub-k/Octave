using KJakub.Octave.Data;
using KJakub.Octave.Editor.Interfaces;
using KJakub.Octave.Managers.AudioFileManager;
using KJakub.Octave.Managers.CommandManager;
using SFB;
using System.IO;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
namespace KJakub.Octave.Editor.Logic
{
    public class NavbarLogic
    {
        private ICommandManager cmdManager;
        private IAudioFileManager audioFileManager;
        private SongData songData;
        private IEditorPopup popup;
        private IEditorSave save;
        public NavbarLogic(ICommandManager cmdManager, IAudioFileManager audioFileManager, SongData songData, IEditorPopup popup, IEditorSave save)
        {
            (this.cmdManager, this.audioFileManager, this.songData, this.popup, this.save) = (cmdManager, audioFileManager, songData, popup, save);
        }
        public void Save()
        {
            save.Show();
        }
        public void Load()
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Load Song", "", "meta", false);

            if (paths.Length == 0)
                return;

            string metaPath = paths[0];
            string directory = Path.GetDirectoryName(metaPath);
            string songName = Path.GetFileNameWithoutExtension(metaPath);

            string jsonPath = Path.Combine(directory, songName + ".json");
            string wavPath = Path.Combine(directory, songName + ".wav");

            if (!File.Exists(jsonPath) || !File.Exists(wavPath))
                return;

            string metaContent = File.ReadAllText(metaPath);
            var metadata = JsonConvert.DeserializeObject<SongMetadata>(metaContent);

            string jsonContent = File.ReadAllText(jsonPath);
            var notesWrapper = JsonConvert.DeserializeObject<NotesWrapper>(jsonContent);

            byte[] wavBytes = File.ReadAllBytes(wavPath);
            AudioClip clip = audioFileManager.ToAudioClip(wavBytes, songName);

            songData.Update(new(clip, metadata.Lines, metadata.BPM, metadata.Snapping, notesWrapper.Notes));
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