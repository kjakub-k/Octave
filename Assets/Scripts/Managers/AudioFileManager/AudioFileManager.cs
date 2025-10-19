using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
namespace KJakub.Octave.Managers.AudioFileManager
{
    public class AudioFileManager : IAudioFileManager
    {
        public AudioClip ToAudioClip(byte[] wavFileBytes, string clipName = "song")
        {
            int headerSize = 44;

            if (wavFileBytes == null || wavFileBytes.Length <= headerSize)
                throw new ArgumentException("Invalid WAV file data");

            int sampleRate = BitConverter.ToInt32(wavFileBytes, 24);
            short channels = BitConverter.ToInt16(wavFileBytes, 22);
            short bitsPerSample = BitConverter.ToInt16(wavFileBytes, 34);

            if (bitsPerSample != 16)
                throw new NotSupportedException("Only 16 bit WAV files are supported");

            int dataSize = wavFileBytes.Length - headerSize;
            int sampleCount = dataSize / 2;
            float[] audioData = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                short sample = BitConverter.ToInt16(wavFileBytes, headerSize + i * 2);
                audioData[i] = sample / 32768f;
            }

            AudioClip audioClip = AudioClip.Create(clipName, sampleCount / channels, channels, sampleRate, false);
            audioClip.SetData(audioData, 0);

            return audioClip;
        }
        public async Task<AudioClip> LoadAudioClip(string path)
        {
            byte[] wavData = await Task.Run(() => File.ReadAllBytes(path));
            AudioClip clip = ToAudioClip(wavData, Path.GetFileNameWithoutExtension(path));
            return clip;
        }
    }
}