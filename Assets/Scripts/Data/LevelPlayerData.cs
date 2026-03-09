using System;
namespace KJakub.Octave.Data
{
    [Serializable]
    public class LevelPlayerData
    {
        public GameStats BestStats { get; set; }
        public int InputOffset { get; set; }
        public int MusicOffset { get; set; }
        public LevelPlayerData(GameStats bestStats, int inputOffset = 0, int musicOffset = 0)
        {
            (BestStats, InputOffset, MusicOffset) = (bestStats, inputOffset, musicOffset);
        }
    }
}