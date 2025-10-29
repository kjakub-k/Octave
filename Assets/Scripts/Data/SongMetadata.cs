using System;
namespace KJakub.Octave.Data
{
    [Serializable]
    public class SongMetadata
    {
        public string SongName { get; set; }
        public string Author { get; set; }
        public string Mapper { get; set; }
        public int BPM { get; set; }
        public int Lines { get; set; }
        public SnappingType Snapping { get; set; }
        public SongMetadata(string songName, string author, string mapper, int bpm, int lines, SnappingType snapping)
        {
            (SongName, Author, Mapper, BPM, Lines, Snapping) = (songName, author, mapper, bpm, lines, snapping);
        }
    }
}