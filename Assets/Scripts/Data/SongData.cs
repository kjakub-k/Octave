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
[Serializable]
public class SongData
{
    public AudioClip Song { get; set; }
    public int Lines { get; set; }
    public float BPM { get; set; }
    public SnappingType Snapping { get; set; }
    public SongData() 
    { 
        Song = null;
        Lines = 4;
        BPM = 120;
        Snapping = SnappingType.Quarter;
    }
    public SongData(AudioClip song, int lines, float bPM, SnappingType snapping)
    {
        Song = song;
        Lines = (lines > 7) ? 7 : (lines < 1) ? 1 : lines;
        BPM = bPM;
        Snapping = snapping;
    }
}