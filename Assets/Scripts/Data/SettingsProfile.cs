using System;
using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.Data
{
    [Serializable]
    public class SettingsProfile
    {
        public float MusicVolume;
        public float SoundVolume;
        public int NoteSpeed;
        public ResolutionData Resolution;
        public int QualityIndex;
        public Dictionary<int, string> RebindsByLaneCount = new();
        public SettingsProfile(float musicVolume, float soundVolume, int noteSpeed, ResolutionData resData, int qualityIndex, Dictionary<int, string> rebindsByLaneCount)
        {
            (MusicVolume, SoundVolume, NoteSpeed, Resolution, QualityIndex, RebindsByLaneCount) = (musicVolume, soundVolume, noteSpeed, resData, qualityIndex, rebindsByLaneCount);
        }
        public void EnsureInitialized()
        {
            if (RebindsByLaneCount == null)
                RebindsByLaneCount = new Dictionary<int, string>();
        }
    }
    [Serializable]
    public class ResolutionData
    {
        public int Width;
        public int Height;
        public ResolutionData(int width, int height)
        {
            (Width, Height) = (width, height);
        }
    }
}