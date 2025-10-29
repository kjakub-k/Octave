using System.Threading.Tasks;
using UnityEngine;
namespace KJakub.Octave.Managers.AudioFileManager
{
    public interface IAudioFileManager
    {
        public Task<AudioClip> LoadAudioClip(string path);
        public AudioClip ToAudioClip(byte[] wav, string name);
        public byte[] ToWav(AudioClip clip);
    }
}