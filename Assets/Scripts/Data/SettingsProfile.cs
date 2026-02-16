using System;
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
        public string RebindsJson;
        public SettingsProfile(float musicVolume, float soundVolume, int noteSpeed, ResolutionData resData, int qualityIndex, string rebindsJson)
        {
            (MusicVolume, SoundVolume, NoteSpeed, Resolution, QualityIndex, RebindsJson) = (musicVolume, soundVolume, noteSpeed, resData, qualityIndex, rebindsJson);
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