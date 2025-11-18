using KJakub.Octave.Data;
using KJakub.Octave.Editor.Interfaces;
using KJakub.Octave.Managers.AudioFileManager;
using KJakub.Octave.Managers.CommandManager.NoteCommandManager;
using KJakub.Octave.Game.Interfaces;
using SFB;
using System.IO;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
namespace KJakub.Octave.Editor.Logic
{
    public class NavbarLogic
    {
        private NoteCommandManager cmdManager;
        private SongData songData;
        private IEditorPopup popup;
        private IEditorSave save;
        private IGameController gameController;
        public NavbarLogic(NoteCommandManager cmdManager, SongData songData, IEditorPopup popup, IEditorSave save, IGameController gameController)
        {
            (this.cmdManager, this.songData, this.popup, this.save, this.gameController) = (cmdManager, songData, popup, save, gameController);
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
            AudioClip clip = AudioFileManager.ToAudioClip(wavBytes, songName);

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
                songData.Song = await AudioFileManager.LoadAudioClip(path);
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
        public void StartGame()
        {
            gameController.StartGame(songData);
        }
    }
}