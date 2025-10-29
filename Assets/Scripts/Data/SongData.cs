using System.Collections.Generic;
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
    public class SongData
    {
        private AudioClip song;
        private int lines;
        private int bpm;
        private SnappingType snapping;
        private List<NoteData> notes;
        public AudioClip Song
        {
            get { return song; }
            set
            {
                song = value;
                OnSongChanged?.Invoke(value);
            }
        }
        public int Lines 
        { 
            get { return lines; }
            set
            {
                lines = (value > 7) ? 7 : (value < 1) ? 1 : value;
                OnLinesChanged?.Invoke(lines);
            }
        }
        public int BPM 
        {
            get { return bpm; }
            set
            {
                OnBPMChanged?.Invoke(value);
                bpm = value;
            }
        }
        public SnappingType Snapping 
        { 
            get { return snapping; } 
            set 
            { 
                OnSnappingChanged?.Invoke(value); 
                snapping = value; 
            } 
        }
        public List<NoteData> Notes { get { return notes; } set { notes = value; } }
        public event Action<AudioClip> OnSongChanged;
        public event Action<int> OnLinesChanged;
        public event Action<int> OnBPMChanged;
        public event Action<SnappingType> OnSnappingChanged;
        public event Action OnSongUpdated; //new song has been loaded in
        public SongData() 
        { 
            Song = null;
            Lines = 4;
            BPM = 40;
            Snapping = SnappingType.Quarter;
            Notes = new List<NoteData>();
        }
        public SongData(AudioClip song, int lines, int bPM, SnappingType snapping)
        {
            Song = song;
            Lines = lines;
            BPM = bPM;
            Snapping = snapping;
            Notes = new List<NoteData>();
        }
        public SongData(AudioClip song, int lines, int bPM, SnappingType snapping, List<NoteData> notes)
        {
            Song = song;
            Lines = lines;
            BPM = bPM;
            Snapping = snapping;
            Notes = notes;
        }
        /// <summary>
        /// Replaces its variables with parameter's.
        /// </summary>
        public void Update(SongData songData)
        {
            Song = songData.Song;
            Lines = songData.Lines;
            BPM = songData.BPM;
            Snapping = songData.Snapping;
            Notes = songData.Notes;
            OnSongUpdated?.Invoke();
        }
    }
}