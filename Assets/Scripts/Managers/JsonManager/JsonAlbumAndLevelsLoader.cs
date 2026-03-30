using KJakub.Octave.Data;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
namespace KJakub.Octave.Managers.JsonManager
{
    public static class JsonAlbumAndLevelsLoader
    {
        private const string AlbumsPath = "Assets/External/Albums";
        private const string SongsFolderName = "Songs";
        public static AlbumData[] LoadAllAlbums()
        {
            if (!Directory.Exists(AlbumsPath))
            {
                Debug.LogWarning($"Albums directory not found: {AlbumsPath}");
                return Array.Empty<AlbumData>();
            }

            List<AlbumData> albums = new List<AlbumData>();

            foreach (string albumDir in Directory.GetDirectories(AlbumsPath))
            {
                string albumJsonPath = Path.Combine(albumDir, "album.json");

                if (!File.Exists(albumJsonPath))
                    continue;

                string albumJson = File.ReadAllText(albumJsonPath);
                AlbumData album = JsonConvert.DeserializeObject<AlbumData>(albumJson);

                if (album == null)
                    continue;

                string coverPath = Path.Combine(albumDir, "cover.png");

                if (File.Exists(coverPath))
                {
                    byte[] fileData = File.ReadAllBytes(coverPath);
                    Texture2D tex = new Texture2D(2, 2);

                    if (tex.LoadImage(fileData))
                        album.CoverImage = tex;
                }

                string songsDir = Path.Combine(albumDir, SongsFolderName);
                album.Levels = LoadLevelsFromFolder(songsDir);

                albums.Add(album);
            }

            return albums.ToArray();
        }
        public static LevelData[] LoadLevelsFromFolder(string songsDirectory)
        {
            if (!Directory.Exists(songsDirectory))
                return Array.Empty<LevelData>();

            List<LevelData> levels = new List<LevelData>();

            foreach (string songFolder in Directory.GetDirectories(songsDirectory))
            {
                string metadataPath = Path.Combine(songFolder, "metadata.json");

                if (!File.Exists(metadataPath))
                    continue;

                string metaJson = File.ReadAllText(metadataPath);
                SongMetadata metadata = JsonConvert.DeserializeObject<SongMetadata>(metaJson);

                if (metadata == null)
                    continue;

                string notesPath = Path.Combine(songFolder, "notes.json");
                NotesWrapper notesWrapper = null;

                if (File.Exists(notesPath))
                {
                    string notesJson = File.ReadAllText(notesPath);
                    notesWrapper = JsonConvert.DeserializeObject<NotesWrapper>(notesJson);
                }

                string wavPath = Path.Combine(songFolder, Path.GetFileName(songFolder) + ".wav");
                AudioClip audioClip = null;

                if (File.Exists(wavPath))
                {
                    byte[] wavBytes = File.ReadAllBytes(wavPath);
                    audioClip = AudioFileManager.AudioFileManager.ToAudioClip(wavBytes);
                }

                LevelData level = new LevelData(metadata, audioClip, notesWrapper);
                levels.Add(level);
            }

            return levels.ToArray();
        }
    }
}
