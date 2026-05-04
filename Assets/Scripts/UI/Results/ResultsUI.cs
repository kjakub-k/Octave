using KJakub.Octave.Data;
using KJakub.Octave.Game.Core;
using KJakub.Octave.UI.Core;
using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using KJakub.Octave.Managers.LanguageManager;
namespace KJakub.Octave.UI.Results 
{ 
    public class ResultsUI : MonoBehaviour
    {
        private SongData songData;
        private SongMetadata songMetadata;
        private Action onGameEnd;
        [SerializeField]
        private GameController gameController;
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private ResultsGraphUI resultsGraphUI;
        [Header("Components")]
        [SerializeField]
        private TMP_Text songLabel;
        [SerializeField]
        private TMP_Text hitsLabel;
        [SerializeField]
        private TMP_Text accuracyLabel;
        [SerializeField]
        private TMP_Text scoreLabel;
        [SerializeField]
        private TMP_Text missesLabel;
        [SerializeField]
        private TMP_Text maxComboLabel;
        [Header("Labels (For Translation)")]
        [SerializeField]
        private TMP_Text hitsTitleLabel;
        [SerializeField]
        private TMP_Text accuracyTitleLabel;
        [SerializeField]
        private TMP_Text scoreTitleLabel;
        [SerializeField]
        private TMP_Text missesTitleLabel;
        [SerializeField]
        private TMP_Text maxComboTitleLabel;
        [SerializeField]
        private TMP_Text returnToMenuBtnLabel;
        [SerializeField]
        private TMP_Text playAgainBtnLabel;
        [SerializeField]
        private TMP_Text watchReplayBtnLabel;
        [SerializeField]
        private TMP_Text hitsGraphLabel;
        [SerializeField]
        private TMP_Text timeGraphLabel;
        private void OnEnable()
        {
            Translate();
        }
        private void Translate()
        {
            hitsTitleLabel.text = LanguageManager.GetTranslation("successful_hits");
            accuracyTitleLabel.text = LanguageManager.GetTranslation("accuracy");
            scoreTitleLabel.text = LanguageManager.GetTranslation("score");
            missesTitleLabel.text = LanguageManager.GetTranslation("misses");
            maxComboLabel.text = LanguageManager.GetTranslation("max_combo");
            returnToMenuBtnLabel.text = LanguageManager.GetTranslation("return_to_menu");
            playAgainBtnLabel.text = LanguageManager.GetTranslation("play_again");
            watchReplayBtnLabel.text = LanguageManager.GetTranslation("watch_replay");
            hitsGraphLabel.text = LanguageManager.GetTranslation("hits");
            timeGraphLabel.text = LanguageManager.GetTranslation("time");
        }
        public void SaveInfo(SongData songData, SongMetadata songMetadata, Action onGameEnd)
        {
            (this.songData, this.songMetadata, this.onGameEnd) = (songData, songMetadata, onGameEnd);
        }
        public void UpdateUI(SongMetadata metadata, GameStats stats)
        {
            songLabel.text = metadata.SongName + $" {LanguageManager.GetTranslation("by")} " + metadata.Author;
            hitsLabel.text = $"{stats.TotalHits}";
            scoreLabel.text = $"{stats.Score}";
            missesLabel.text = $"{stats.Misses}";
            maxComboLabel.text = $"{stats.HighestCombo}";
            accuracyLabel.text = $"{stats.GetAccuracyPercentage():0.00}%";
            resultsGraphUI.DrawOnGraph(stats.HitsAccuracy, stats.LevelLength);
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
            SaveInfo(songData, songMetadata, () =>
            {
                uiController.ShowResults();
                uiController.HideGame();
                UpdateUI(songMetadata, gameController.GameStats);
            });
            gameController.OnFinished += OnGameEnd;
            gameController.PlayGame(songData);
        }
        public void WatchReplay()
        {
            uiController.HideResults();
            uiController.ShowGame();
            SaveInfo(songData, songMetadata, () =>
            {
                uiController.ShowResults();
                uiController.HideGame();
            });
            gameController.OnFinished += OnGameEnd;
            gameController.PlayGame(songData, gameController.Presses);
        }
    }
}