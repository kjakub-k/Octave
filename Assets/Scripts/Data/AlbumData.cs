using System;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace KJakub.Octave.Data
{
    [Serializable]
    public class AlbumData
    {
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public int Difficulty { get; set; }
        [JsonIgnore]
        public Texture2D CoverImage { get; set; }
        [JsonIgnore]
        public LevelData[] Levels { get; set; } = Array.Empty<LevelData>();
    }
}