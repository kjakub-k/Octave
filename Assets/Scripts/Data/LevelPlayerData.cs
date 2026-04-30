using System;
namespace KJakub.Octave.Data
{
    [Serializable]
    public class LevelPlayerData
    {
        public int InputOffset { get; set; }
        public int MusicOffset { get; set; }
        public LevelPlayerData(int inputOffset = 0, int musicOffset = 0)
        {
            (InputOffset, MusicOffset) = (inputOffset, musicOffset);
        }
    }
}