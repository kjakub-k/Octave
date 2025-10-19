using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using KJakub.Octave.Managers.CommandManager;
using KJakub.Octave.Data;
using KJakub.Octave.CustomElements;
namespace KJakub.Octave.Editor.UI
{
    public class NavbarUI
    {
        private VisualElement navbar;
        public NavbarUI(VisualElement root, CommandManager cmdManager, EditorPopupUI popup, SongData songData)
        {
            navbar = root.Q<VisualElement>("Navbar");

            AddOptionsToDropdownMenu("File", new() { 
                ("Save File", () => { Debug.Log("Ulozeno"); }), 
                ("Load File", () => { Debug.Log("Nacteno"); }) 
            });
            AddOptionsToDropdownMenu("Edit", new() { 
                ("Undo", () => { cmdManager.Undo(); }), 
                ("Redo", () => { cmdManager.Redo(); }) 
            });
            AddOptionsToDropdownMenu("Song", new() { 
                ("Set Song", async () => { songData.Song = await CreateSong(); }),
                ("Set BPM", async () => { songData.BPM = await popup.CreatePopupAsync<int>(); }),
                ("Set Lines Amount", async () => { songData.Lines = await popup.CreatePopupAsync<int>(); }),
                ("Set Snapping", async () => { songData.Snapping = await popup.CreatePopupAsync<SnappingType>(); })
            });
        }
        private void AddOptionsToDropdownMenu(string elementName, List<(string, Action)> options)
        {
            DropdownButton ddBtn = navbar.Q<DropdownButton>(elementName);
        
            foreach (var option in options)
            {
                ddBtn.AddOption(option.Item1, option.Item2);
            }
        }
        private Task<AudioClip> CreateSong()
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Select WAV File", "", "wav", false);
            var path = paths[0];

            return LoadWavAndSetResult(path);
        }
        private AudioClip ToAudioClip(byte[] wavFileBytes, string clipName = "song")
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
        private async Task<AudioClip> LoadWavAndSetResult(string filePath)
        {
            byte[] wavData = await Task.Run(() => File.ReadAllBytes(filePath));
            AudioClip clip = ToAudioClip(wavData, Path.GetFileNameWithoutExtension(filePath));
            return clip;
        }
    }
}