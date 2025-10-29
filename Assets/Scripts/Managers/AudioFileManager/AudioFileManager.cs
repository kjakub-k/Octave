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
        public byte[] ToWav(AudioClip clip)
        {
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);

            short[] intData = new short[samples.Length];
            byte[] bytesData = new byte[samples.Length * 2];
            const float rescaleFactor = 32767f;

            for (int i = 0; i < samples.Length; i++)
            {
                intData[i] = (short)(samples[i] * rescaleFactor);
                byte[] byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            int sampleCount = samples.Length;
            int byteRate = clip.frequency * clip.channels * 2;

            using (MemoryStream stream = new MemoryStream(44 + bytesData.Length))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
                writer.Write(36 + bytesData.Length);
                writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));
                writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)clip.channels);
                writer.Write(clip.frequency);
                writer.Write(byteRate);
                writer.Write((ushort)(clip.channels * 2));
                writer.Write((ushort)16);
                writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
                writer.Write(bytesData.Length);
                writer.Write(bytesData);
                writer.Flush();
                return stream.ToArray();
            }
        }
    }
}