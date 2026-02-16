using KJakub.Octave.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KJakub.Octave.UI.Core;
using UnityEngine.InputSystem;
using KJakub.Octave.Game.Core;
using KJakub.Octave.UI.Results;
namespace KJakub.Octave.UI.LevelSelect
{
    public class LevelSelectUI : MonoBehaviour 
    {
        private AlbumData album;
        private int currentSongIndex = 0;
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private GameController gameController;
        [SerializeField]
        private GameObject levelInfoPrefab;
        [SerializeField]
        private ResultsUI resultsUI;
        [SerializeField]
        private PlayerInput inputSystem;
        [Header("Components")]
        [SerializeField]
        private RawImage image;
        [SerializeField]
        private TMP_Text songNameLabel;
        [SerializeField]
        private Transform content;
        private void Start()
        {
            inputSystem.actions.FindAction("Navigate").performed += (InputAction.CallbackContext c) => { OnNavigate(c.ReadValue<Vector2>()); } ;
        }
        private void EndGame()
        {
            uiController.HideGame();
            uiController.ShowResults();
            resultsUI.UpdateUI(album.Levels[currentSongIndex].Metadata, gameController.GameStats);
            gameController.OnFinished -= EndGame;
        }
        public void Initialize(AlbumData album)
        {
            this.album = album;
            currentSongIndex = 0;
            image.texture = album.CoverImage;
            UpdateSongLabel(album.Levels[currentSongIndex].Metadata.SongName);
            GenerateLevels(album.Levels);
        }
        private void UpdateSongLabel(string newText)
        {
            songNameLabel.text = newText;
        }
        private void GenerateLevels(LevelData[] levels)
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < levels.Length; i++)
            {
                var level = levels[i];
                var pref = Instantiate(levelInfoPrefab, content);
                pref.GetComponentInChildren<TMP_Text>().text = $"{level.Metadata.SongName} by {level.Metadata.Author}";
                LevelItemUI lvlItemUI = pref.GetComponent<LevelItemUI>();

                lvlItemUI.LevelSelectUI = this;
                lvlItemUI.IndexToGive = i;

                if (i == 0)
                {
                    pref.GetComponent<LevelItemUI>().UpdateSelection();
                }
            }
        }
        public void OnNavigate(Vector2 input)
        {
            if (input.y > 0)
                currentSongIndex--;
            else if (input.y < 0)
                currentSongIndex++;

            currentSongIndex = Mathf.Clamp(currentSongIndex, 0, album.Levels.Length - 1);

            UpdateSelection();
            UpdateSongLabel(album.Levels[currentSongIndex].Metadata.SongName);
        }
        public void OnLevelPressed(int newIndex)
        {
            currentSongIndex = newIndex;
            UpdateSelection();
        }
        public void Play()
        {
            var level = album.Levels[currentSongIndex];
            var songData = new SongData(level.Song, level.Metadata.Lines, level.Metadata.BPM, level.Metadata.Snapping, level.Notes.Notes);

            uiController.HideLevelSelectionMenu();
            uiController.ShowGame();
            resultsUI.SaveInfo(songData, level.Metadata, EndGame);
            gameController.OnFinished += EndGame;
            gameController.PlayGame(songData);
        }
        private void UpdateSelection()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                if (i == currentSongIndex)
                {
                    content.GetChild(i).GetComponent<LevelItemUI>().UpdateSelection();
                }
                else
                {
                    content.GetChild(i).GetComponent<LevelItemUI>().NotSelected();
                }
            }
        }
        public void BackToAlbumSelection()
        {
            uiController.ShowAlbumSelectionMenu();
            uiController.HideLevelSelectionMenu();
        }
    }
}