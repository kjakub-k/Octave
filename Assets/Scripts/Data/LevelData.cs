using System;
using UnityEngine;
namespace KJakub.Octave.Data
{
    [Serializable]
    public class LevelData
    {
        public NotesWrapper Notes { get; set; }
        public SongMetadata Metadata { get; set; }
        public AudioClip Song { get; set; }
        public LevelData(SongMetadata metadata, AudioClip song, NotesWrapper notes)
        {
            (Metadata, Song, Notes) = (metadata, song, notes);
        }
    }
}