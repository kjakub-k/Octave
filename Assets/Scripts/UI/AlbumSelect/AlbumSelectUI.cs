using KJakub.Octave.Data;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using KJakub.Octave.UI.Core;
using KJakub.Octave.UI.LevelSelect;
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
        public void Initialize(AlbumData[] albums)
        {
            this.albums = albums;
            SwitchAlbum(albums[currentIndex]);
            UpdateSelection();
        }
        private void SwitchAlbum(AlbumData album)
        {
            albumLabel.text = $"{album.AlbumName} by {album.ArtistName}";
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
