using System;
using UnityEngine;
public enum SnappingType
{
    Full = 1,
    Half = 2,
    Quarter = 4,
    Eighth = 8,
    Sixteenth = 16
}
namespace KJakub.Octave.Data
{
    [Serializable]
    public class SongData
    {
        private AudioClip song;
        private int lines;
        private int bpm;
        private SnappingType snapping;
        public AudioClip Song
        {
            get
            {
                return song;
            }
            set
            {
                OnSongChanged?.Invoke(value);
                song = value;
            }
        }
        public int Lines 
        { 
            get
            {
                return lines;
            }
            set
            {
                OnLinesChanged?.Invoke(value);
                lines = value;
            }
        }
        public int BPM 
        {
            get
            {
                return bpm;
            }
            set
            {
                OnBPMChanged?.Invoke(value);
                bpm = value;
            }
        }
        public SnappingType Snapping 
        { 
            get 
            { 
                return snapping; 
            } 
            set 
            { 
                OnSnappingChanged?.Invoke(value); 
                snapping = value; 
            } 
        }
        public event Action<AudioClip> OnSongChanged;
        public event Action<int> OnLinesChanged;
        public event Action<int> OnBPMChanged;
        public event Action<SnappingType> OnSnappingChanged;
        public SongData() 
        { 
            Song = null;
            Lines = 4;
            BPM = 120;
            Snapping = SnappingType.Quarter;
        }
        public SongData(AudioClip song, int lines, int bPM, SnappingType snapping)
        {
            Song = song;
            Lines = (lines > 7) ? 7 : (lines < 1) ? 1 : lines;
            BPM = bPM;
            Snapping = snapping;
        }
    }
}