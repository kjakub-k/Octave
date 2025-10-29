using KJakub.Octave.Data;
using KJakub.Octave.Managers.AudioFileManager;
using SFB;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
namespace KJakub.Octave.Editor.Logic
{
    public class SaveLogic
    {
        private IAudioFileManager audioFileManager;
        public SaveLogic(IAudioFileManager audioFileManager)
        {
            this.audioFileManager = audioFileManager;
        }
        public void Save(SongData songData, SongMetadata metadata)
        {
            string defaultName = metadata.SongName ?? "Song";
            string savePath = StandaloneFileBrowser.SaveFilePanel("Save Song As", "", defaultName, "meta");

            if (string.IsNullOrEmpty(savePath))
                return;

            string directory = Path.GetDirectoryName(savePath);
            string songName = Path.GetFileNameWithoutExtension(savePath);

            string metaPath = Path.Combine(directory, songName + ".meta");
            string jsonPath = Path.Combine(directory, songName + ".json");
            string wavPath = Path.Combine(directory, songName + ".wav");

            string metaJson = JsonConvert.SerializeObject(metadata);
            File.WriteAllText(metaPath, metaJson);

            string notesJson = JsonConvert.SerializeObject(songData.Notes);
            File.WriteAllText(jsonPath, notesJson);

            byte[] wavBytes = audioFileManager.ToWav(songData.Song);
            File.WriteAllBytes(wavPath, wavBytes);
        }
    }
}