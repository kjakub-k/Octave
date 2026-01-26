using KJakub.Octave.Data;
using KJakub.Octave.Game.Core;
using KJakub.Octave.UI.Core;
using System;
using TMPro;
using UnityEngine;
namespace KJakub.Octave.UI.Results 
{ 
    public class ResultsUI : MonoBehaviour
    {
        private SongData songData;
        private Action onGameEnd;
        [SerializeField]
        private GameController gameController;
        [SerializeField]
        private UIController uiController;
        [Header("Components")]
        [SerializeField]
        private TMP_Text songLabel;
        public void SaveInfo(SongData songData, Action onGameEnd)
        {
            (this.songData, this.onGameEnd) = (songData, onGameEnd);
        }
        public void UpdateUI(SongMetadata metadata)
        {
            songLabel.text = metadata.SongName + " by " + metadata.Author;
        }
        public void ReturnToMenu()
        {
            uiController.HideResults();
            uiController.ShowLevelSelectionMenu();
        }
        private void OnGameEnd()
        {
            onGameEnd?.Invoke();
            gameController.OnFinished -= OnGameEnd;
        }
        public void PlayAgain()
        {
            uiController.HideResults();
            uiController.ShowGame();
            gameController.OnFinished += OnGameEnd;
            gameController.PlayGame(songData);
        }
        public void WatchReplay()
        {
            uiController.HideResults();
            uiController.ShowGame();
            gameController.OnFinished += OnGameEnd;
            gameController.PlayGame(songData, gameController.Presses);
        }
    }
}