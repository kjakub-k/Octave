using KJakub.Octave.Data;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using KJakub.Octave.UI.Core;
using KJakub.Octave.UI.LevelSelect;
using KJakub.Octave.Managers.LanguageManager;
namespace KJakub.Octave.UI.AlbumSelect
{
    public class AlbumSelectUI : MonoBehaviour
    {
        private AlbumData[] albums;
        private int currentIndex = 0;
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private LevelSelectUI levelSelectUI;
        [SerializeField]
        private AlbumMoveButtonUI[] buttons;
        [Header("Components")]
        [SerializeField]
        private TMP_Text albumLabel;
        [SerializeField]
        private RawImage albumImage;
        [Header("Labels (For Translation)")]
        [SerializeField]
        private TMP_Text backBtnLabel;
        [SerializeField]
        private TMP_Text playBtnLabel;
        private void OnEnable()
        {
            Translation();
        }
        public void Initialize(AlbumData[] albums)
        {
            this.albums = albums;
            SwitchAlbum(albums[currentIndex]);
            UpdateSelection();
        }
        private void Translation()
        {
            backBtnLabel.text = LanguageManager.GetTranslation("back_btn");
            playBtnLabel.text = LanguageManager.GetTranslation("play_btn");
        }
        private void SwitchAlbum(AlbumData album)
        {
            albumLabel.text = $"{album.AlbumName} ({album.ArtistName})";
            albumImage.texture = album.CoverImage;
        }
        public void SwitchToLevelSelection()
        {
            uiController.ShowLevelSelectionMenu();
            levelSelectUI.Initialize(albums[currentIndex]);
            uiController.HideAlbumSelectionMenu();
        }
        public void BackToMenu()
        {
            uiController.ShowMainMenu();
            uiController.HideAlbumSelectionMenu();
        }
        private void UpdateSelection()
        {
            if (currentIndex - 1 < 0)
                buttons[0].UpdateSelection(false);
            else
                buttons[0].UpdateSelection(true);

            if (currentIndex + 1 >= albums.Count())
                buttons[1].UpdateSelection(false);
            else
                buttons[1].UpdateSelection(true);
        }
        public void DecreaseAlbumIndex()
        {
            if (currentIndex - 1 < 0)
                return;

            currentIndex--;

            SwitchAlbum(albums[currentIndex]);
            UpdateSelection();
        }
        public void IncreaseAlbumIndex()
        {
            if (currentIndex + 1 >= albums.Count())
                return;

            currentIndex++;

            SwitchAlbum(albums[currentIndex]);
            UpdateSelection();
        }
    }
}
