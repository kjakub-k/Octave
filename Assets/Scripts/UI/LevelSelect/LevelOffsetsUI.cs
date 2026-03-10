using KJakub.Octave.Data;
using TMPro;
using UnityEngine;

namespace KJakub.Octave.UI.LevelSelect
{
    public class LevelOffsetsUI : MonoBehaviour
    {
        [Header("Music Offset")]
        [SerializeField]
        private TMP_Text musicOffsetLabel;
        [Header("Input Offset")]
        [SerializeField]
        private TMP_Text inputOffsetLabel;
        public void UpdateAllLabels(LevelPlayerData lpd)
        {
            UpdateMusicOffset(lpd);
            UpdateInputOffset(lpd);
        }
        public void UpdateMusicOffset(LevelPlayerData lpd)
        {
            musicOffsetLabel.text = lpd.MusicOffset + "ms";
        }
        public void UpdateInputOffset(LevelPlayerData lpd)
        {
            inputOffsetLabel.text = lpd.InputOffset + "ms";
        }
        public LevelPlayerData ChangeInputOffset(LevelPlayerData lpd, int value)
        {
            lpd.InputOffset += value;
            UpdateInputOffset(lpd);
            return lpd;
        }
        public LevelPlayerData ChangeMusicOffset(LevelPlayerData lpd, int value)
        {
            lpd.MusicOffset += value;
            UpdateMusicOffset(lpd);
            return lpd;
        }
    }
}