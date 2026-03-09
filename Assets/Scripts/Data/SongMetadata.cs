using System;
namespace KJakub.Octave.Data
{
    [Serializable]
    public class SongMetadata
    {
        public int Priority { get; set; } // how much it should be prioritized in sorting related to visual representation of song
        public string ID { get; set; }
        public string SongName { get; set; }
        public string Author { get; set; }
        public string Mapper { get; set; }
        public int BPM { get; set; }
        public int Lines { get; set; }
        public SnappingType Snapping { get; set; }
        public SongMetadata(string id, string songName, string author, string mapper, int bpm, int lines, SnappingType snapping, int priority = 0)
        {
            (ID, SongName, Author, Mapper, BPM, Lines, Snapping, Priority) = (id, songName, author, mapper, bpm, lines, snapping, priority);
        }
    }
}