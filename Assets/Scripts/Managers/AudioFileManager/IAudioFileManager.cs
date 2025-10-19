using System.Threading.Tasks;
using UnityEngine;
namespace KJakub.Octave.Managers.AudioFileManager
{
    public interface IAudioFileManager
    {
        public Task<AudioClip> LoadAudioClip(string path);
    }
}