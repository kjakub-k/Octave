using KJakub.Octave.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KJakub.Octave.UI.Core;
using UnityEngine.InputSystem;
using KJakub.Octave.Game.Core;
using KJakub.Octave.UI.Results;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using KJakub.Octave.Managers.GamejoltManager;
using KJakub.Octave.Managers.LanguageManager;
using KJakub.Octave.Managers.AchievementsManager;
using KJakub.Octave.Managers.SettingsManager;
using KJakub.Octave.Managers.AttemptsManager;
namespace KJakub.Octave.UI.LevelSelect
{
    public class LevelSelectUI : MonoBehaviour 
    {
        private AlbumData album;
        private int currentSongIndex = 0;
        private List<GameModifier> activeModifiers = new();
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private GameController gameController;
        [SerializeField]
        private PracticeController practiceController;
        [SerializeField]
        private AchievementsManager achievementsManager;
        [SerializeField]
        private GameObject levelInfoPrefab;
        [SerializeField]
        private ResultsUI resultsUI;
        [SerializeField]
        private LevelOffsetsUI levelOffsetsUI;
        [SerializeField]
        private PlayerInput inputSystem;
        [SerializeField]
        private SettingsManager settingsManager;
        [SerializeField]
        private LeaderboardUI leaderboardUI;
        [Header("Components")]
        [SerializeField]
        private RawImage image;
        [SerializeField]
        private TMP_Text songNameLabel;
        [SerializeField]
        private Transform content;
        [Header("Labels (For Translation)")]
        [SerializeField]
        private TMP_Text backBtnLabel;
        [SerializeField]
        private TMP_Text playBtnLabel;
        [SerializeField]
        private TMP_Text offsetsLabel;
        [SerializeField]
        private TMP_Text inputOffsetLabel;
        [SerializeField]
        private TMP_Text musicOffsetLabel;
        [SerializeField]
        private TMP_Text practiceBtnLabel;
        [SerializeField]
        private TMP_Text modifierBtnLabel;
        [Header("Modificator Labels (For Translation)")]
        [SerializeField]
        private TMP_Text mirroredTitleLabel;
        [SerializeField]
        private TMP_Text mirroredDescLabel;
        [SerializeField]
        private TMP_Text noMissTitleLabel;
        [SerializeField]
        private TMP_Text noMissDescLabel;
        [SerializeField]
        private TMP_Text endlessTitleLabel;
        [SerializeField]
        private TMP_Text endlessDescLabel;
        [SerializeField]
        private TMP_Text doubleSpeedTitleLabel;
        [SerializeField]
        private TMP_Text doubleSpeedDescLabel;
        [SerializeField]
        private TMP_Text halfSpeedTitleLabel;
        [SerializeField]
        private TMP_Text halfSpeedDescLabel;
        [SerializeField]
        private TMP_Text closeBtnLabel;
        [Header("Leaderboard Labels (For Translation)")]
        [SerializeField]
        private TMP_Text localLabel;
        [SerializeField]
        private TMP_Text friendsLabel;
        [SerializeField]
        private TMP_Text globalLabel;
        [SerializeField]
        private TMP_Text leaderboardCloseBtnLabel;
        [SerializeField]
        private TMP_Text leaderboardBtnLabel;
        public List<GameModifier> ActiveModifiers { get { return activeModifiers; } set { activeModifiers = value; } }
        private void OnEnable()
        {
            Translate();
        }
        private void Start()
        {
            inputSystem.actions.FindAction("Navigate").performed += (InputAction.CallbackContext c) => { OnNavigate(c.ReadValue<Vector2>()); } ;
        }
        private void Translate()
        {
            backBtnLabel.text = LanguageManager.GetTranslation("back_btn");
            playBtnLabel.text = LanguageManager.GetTranslation("play_btn");
            offsetsLabel.text = LanguageManager.GetTranslation("offsets");
            inputOffsetLabel.text = LanguageManager.GetTranslation("input_offset");
            musicOffsetLabel.text = LanguageManager.GetTranslation("music_offset");
            practiceBtnLabel.text = LanguageManager.GetTranslation("practice_btn");
            modifierBtnLabel.text = LanguageManager.GetTranslation("modifiers_btn");

            closeBtnLabel.text = LanguageManager.GetTranslation("close");
            mirroredTitleLabel.text = LanguageManager.GetTranslation("mirrored_title");
            mirroredDescLabel.text = LanguageManager.GetTranslation("mirrored_desc");
            noMissTitleLabel.text = LanguageManager.GetTranslation("nomiss_title");
            noMissDescLabel.text = LanguageManager.GetTranslation("nomiss_desc");
            endlessTitleLabel.text = LanguageManager.GetTranslation("endless_title");
            endlessDescLabel.text = LanguageManager.GetTranslation("endless_desc");
            doubleSpeedTitleLabel.text = LanguageManager.GetTranslation("doublespeed_title");
            doubleSpeedDescLabel.text = LanguageManager.GetTranslation("doublespeed_desc");
            halfSpeedTitleLabel.text = LanguageManager.GetTranslation("halfspeed_title");
            halfSpeedDescLabel.text = LanguageManager.GetTranslation("halfspeed_desc");

            localLabel.text = LanguageManager.GetTranslation("local");
            friendsLabel.text = LanguageManager.GetTranslation("friends");
            globalLabel.text = LanguageManager.GetTranslation("global");
            leaderboardBtnLabel.text = LanguageManager.GetTranslation("leaderboard");
            leaderboardCloseBtnLabel.text = LanguageManager.GetTranslation("close");
        }
        private void EndGame()
        {
            uiController.HideGame();
            uiController.ShowResults();
            gameController.GameStats.AddResultsToPlayerPrefs();
            //UpdateStats(album.Levels[currentSongIndex].Metadata.ID, gameController.GameStats);
            AttemptsManager.Instance.Save(album.Levels[currentSongIndex].Metadata.ID, gameController.GameStats.Score);

            if (PlayerPrefs.GetString("username") != "" && PlayerPrefs.GetString("token") != "")
            {
                //_ = GamejoltLoader.Instance.AddScore(gameController.GameStats.Score.ToString(), gameController.GameStats.Score, album.Levels[currentSongIndex].Metadata.TableID);
                SaveResultsToGamejolt(PlayerPrefs.GetInt("TotalScore"), PlayerPrefs.GetFloat("AverageAccuracy"));
            }

            achievementsManager.CheckEligibleAchievements();
            resultsUI.UpdateUI(album.Levels[currentSongIndex].Metadata, gameController.GameStats);
            gameController.OnFinished -= EndGame;
        }
        private async void SaveResultsToGamejolt(int score, float accuracy)
        {
            GamejoltLoader.Instance.SavePerformance(score, accuracy);
        }
        public void OnInputOffsetChanged(int value)
        {
            string path = $"{Application.persistentDataPath}/LevelPlayerData/{album.Levels[currentSongIndex].Metadata.ID}.json";
            LevelPlayerData lpd = ReturnLevelPlayerData(path);
            levelOffsetsUI.ChangeInputOffset(lpd, value);
            SaveLevelPlayerData(path, lpd);
        }
        public void OnMusicOffsetChanged(int value)
        {
            string path = $"{Application.persistentDataPath}/LevelPlayerData/{album.Levels[currentSongIndex].Metadata.ID}.json";
            LevelPlayerData lpd = ReturnLevelPlayerData(path);
            levelOffsetsUI.ChangeMusicOffset(lpd, value);
            SaveLevelPlayerData(path, lpd);
        }
        /*
        private void UpdateStats(string levelId, GameStats gameStats)
        {
            string path = $"{Application.persistentDataPath}/LevelPlayerData/{levelId}.json";
            LevelPlayerData lpd = ReturnLevelPlayerData(path);

            if (lpd.BestStats.Score < gameStats.Score)
            {
                lpd.BestStats = gameStats;
                SaveLevelPlayerData(path, lpd);
            }
        }
        */
        public void Initialize(AlbumData album)
        {
            this.album = album;
            currentSongIndex = 0;
            image.texture = album.CoverImage;
            UpdateSongLabel(album.Levels[currentSongIndex].Metadata.SongName);
            GenerateLevels(album.Levels);
            levelOffsetsUI.UpdateAllLabels(ReturnLevelPlayerData($"{Application.persistentDataPath}/LevelPlayerData/{album.Levels[currentSongIndex].Metadata.ID}.json"));
            leaderboardUI.LevelID = album.Levels[currentSongIndex].Metadata.ID;
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
                pref.GetComponentInChildren<TMP_Text>().text = $"{level.Metadata.SongName} {LanguageManager.GetTranslation("by")} {level.Metadata.Author}";
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
            leaderboardUI.LevelID = album.Levels[currentSongIndex].Metadata.ID;

            UpdateSelection();
            UpdateSongLabel(album.Levels[currentSongIndex].Metadata.SongName);
            levelOffsetsUI.UpdateAllLabels(ReturnLevelPlayerData($"{Application.persistentDataPath}/LevelPlayerData/{album.Levels[currentSongIndex].Metadata.ID}.json"));
        }
        private LevelPlayerData ReturnLevelPlayerData(string path)
        {
            string json;
            LevelPlayerData lpd;

            if (!Directory.Exists($"{Application.persistentDataPath}/LevelPlayerData"))
                Directory.CreateDirectory($"{Application.persistentDataPath}/LevelPlayerData");

            if (!File.Exists(path))
            {
                lpd = new(0);
                json = JsonConvert.SerializeObject(lpd);
                File.WriteAllText(path, json);
                return lpd;
            }

            json = File.ReadAllText(path);
            lpd = JsonConvert.DeserializeObject<LevelPlayerData>(json);
            return lpd;
        }
        private void SaveLevelPlayerData(string path, LevelPlayerData newLPD)
        {
            string json;

            json = JsonConvert.SerializeObject(newLPD);
            File.WriteAllText(path, json);
        }
        public void OnLevelPressed(int newIndex)
        {
            currentSongIndex = newIndex;
            leaderboardUI.LevelID = album.Levels[currentSongIndex].Metadata.ID;
            UpdateSelection();
        }
        public void Practice()
        {
            var level = album.Levels[currentSongIndex];
            var songData = new SongData(level.Song, level.Metadata.Lines, level.Metadata.BPM, level.Metadata.Snapping, level.Notes.Notes);

            settingsManager.ApplyKeybindsForLaneCount(level.Metadata.Lines);
            uiController.ShowPracticeMode();
            uiController.HideLevelSelectionMenu();
            practiceController.StartPractice(songData, ReturnLevelPlayerData($"{Application.persistentDataPath}/LevelPlayerData/{album.Levels[currentSongIndex].Metadata.ID}.json"));
        }
        public void Play()
        {
            var level = album.Levels[currentSongIndex];
            var songData = new SongData(level.Song, level.Metadata.Lines, level.Metadata.BPM, level.Metadata.Snapping, level.Notes.Notes);

            gameController.NoteSpeed = settingsManager.CurrentProfile.NoteSpeed;
            settingsManager.ApplyKeybindsForLaneCount(level.Metadata.Lines);
            uiController.HideLevelSelectionMenu();
            uiController.ShowGame();
            resultsUI.SaveInfo(songData, level.Metadata, EndGame);
            gameController.OnFinished += EndGame;
            var lpd = ReturnLevelPlayerData($"{Application.persistentDataPath}/LevelPlayerData/{album.Levels[currentSongIndex].Metadata.ID}.json");
            gameController.PlayGame(songData, null, lpd.InputOffset, lpd.MusicOffset, activeModifiers);
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