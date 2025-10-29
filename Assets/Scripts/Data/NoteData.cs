using System;
namespace KJakub.Octave.Data
{
    [Serializable]
    public class NoteData
    {
        public float Time { get; set; }
        public int Lane { get; set; }
    }
}