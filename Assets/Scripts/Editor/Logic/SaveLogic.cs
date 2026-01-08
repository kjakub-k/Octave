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
        public void Save(SongData songData, SongMetadata metadata)
        {
            string defaultName = metadata.SongName ?? "Song";
            string savePath = StandaloneFileBrowser.SaveFilePanel("Save Song As", "", defaultName, "json");

            if (string.IsNullOrEmpty(savePath))
                return;

            string directory = Path.GetDirectoryName(savePath);
            string songName = Path.GetFileNameWithoutExtension(savePath);

            string metaPath = Path.Combine(directory, "metadata.json");
            string metaJson = JsonConvert.SerializeObject(metadata, Formatting.Indented);
            File.WriteAllText(metaPath, metaJson);

            string notesJsonPath = Path.Combine(directory, "notes.json");
            string notesJson = JsonConvert.SerializeObject(new NotesWrapper(songData.Notes), Formatting.Indented);
            File.WriteAllText(notesJsonPath, notesJson);

            string wavPath = Path.Combine(directory, songName + ".wav");
            byte[] wavBytes = AudioFileManager.ToWav(songData.Song);
            File.WriteAllBytes(wavPath, wavBytes);
        }
    }
}