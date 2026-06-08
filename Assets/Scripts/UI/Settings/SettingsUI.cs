using KJakub.Octave.Data;
using KJakub.Octave.Game.Core;
using KJakub.Octave.Managers.LanguageManager;
using KJakub.Octave.Managers.SettingsManager;
using KJakub.Octave.UI.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Settings
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private UIController uiController;
        [SerializeField] private SettingsManager settingsManager;
        [SerializeField] private InputController inputController;
        [SerializeField] private int currentLaneCount = 4;
        [Header("Audio")]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundVolumeSlider;
        [Header("Gameplay")]
        [SerializeField] private Slider noteSpeedSlider;
        [SerializeField] private TMP_Text noteSpeedValueText;
        [Header("Video")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [Header("Profiles")]
        [SerializeField] private TMP_Dropdown profileDropdown;
        [Header("Labels (For Translation)")]
        [SerializeField] private TMP_Text backBtnLabel;
        [SerializeField] private TMP_Text musicVolumeLabel;
        [SerializeField] private TMP_Text soundVolumeLabel;
        [SerializeField] private TMP_Text qualityLabel;
        [SerializeField] private TMP_Text resolutionLabel;
        [SerializeField] private TMP_Text noteSpeedLabel;
        [SerializeField] private TMP_Text audioLabel;
        [SerializeField] private TMP_Text videoLabel;
        [SerializeField] private TMP_Text gameLabel;
        [SerializeField] private TMP_Text profileLabel;
        [SerializeField] private TMP_Text createProfileLabel;
        [SerializeField] private TMP_Text removeProfileLabel;
        [SerializeField] private TMP_Text setKeybindsLabel;
        private Resolution[] availableResolutions;
        private void OnEnable()
        {
            Translate();
        }
        private void Start()
        {
            SetupQualityDropdown();
            SetupResolutionDropdown();
            SetupProfileDropdown();
            LoadCurrentProfileIntoUI();
        }
        private void Translate()
        {
            backBtnLabel.text = LanguageManager.GetTranslation("back_btn");
            musicVolumeLabel.text = LanguageManager.GetTranslation("music_volume_settings");
            soundVolumeLabel.text = LanguageManager.GetTranslation("sound_volume_settings");
            qualityLabel.text = LanguageManager.GetTranslation("quality_settings");
            resolutionLabel.text = LanguageManager.GetTranslation("resolution_settings");
            noteSpeedLabel.text = LanguageManager.GetTranslation("note_speed_settings");
            audioLabel.text = LanguageManager.GetTranslation("audio");
            videoLabel.text = LanguageManager.GetTranslation("video");
            gameLabel.text = LanguageManager.GetTranslation("game");
            profileLabel.text = LanguageManager.GetTranslation("profiles_settings");
            createProfileLabel.text = LanguageManager.GetTranslation("create_profile");
            removeProfileLabel.text = LanguageManager.GetTranslation("remove_profile");
            setKeybindsLabel.text = LanguageManager.GetTranslation("set_keybinds");
        }
        public void SetLaneCount(int count)
        {
            currentLaneCount = count;
            settingsManager.ApplyKeybindsForLaneCount(count);
        }
        public int GetLaneCount()
        {
            return currentLaneCount;
        }
        public void BackToMenu()
        {
            settingsManager.SaveSettingsToJSON();
            uiController.ShowMainMenu();
            uiController.HideSettings();
        }
        private void SetupQualityDropdown()
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        }
        private void SetupResolutionDropdown()
        {
            availableResolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            var options = new List<string>();
            foreach (var res in availableResolutions)
                options.Add($"{res.width} x {res.height}");

            resolutionDropdown.AddOptions(options);
        }
        private void SetupProfileDropdown()
        {
            profileDropdown.ClearOptions();

            var options = new List<string>();
            for (int i = 0; i < settingsManager.Profiles.Count; i++)
                options.Add($"{LanguageManager.GetTranslation("profile")} {i + 1}");

            profileDropdown.AddOptions(options);
            profileDropdown.value = settingsManager.SafeProfileIndex;
        }
        public void LoadCurrentProfileIntoUI()
        {
            var profile = settingsManager.CurrentProfile;

            musicVolumeSlider.SetValueWithoutNotify(profile.MusicVolume);
            soundVolumeSlider.SetValueWithoutNotify(profile.SoundVolume);

            noteSpeedSlider.SetValueWithoutNotify(profile.NoteSpeed);
            noteSpeedValueText.text = profile.NoteSpeed.ToString();

            qualityDropdown.SetValueWithoutNotify(profile.QualityIndex);

            resolutionDropdown.SetValueWithoutNotify(FindResolutionIndex(profile.Resolution));

            int laneCount = currentLaneCount;

            if (profile.RebindsByLaneCount.TryGetValue(laneCount, out InputBindingSet set))
            {
                inputController.ClearAllBindings();
                inputController.LoadBindingGroup(set.KeyboardJSON);
                inputController.LoadBindingGroup(set.GamepadJSON);
            }
            else
            {
                inputController.ClearAllBindings();
            }
        }
        public void ApplySettings()
        {
            var profile = settingsManager.CurrentProfile;

            profile.MusicVolume = musicVolumeSlider.value;
            profile.SoundVolume = soundVolumeSlider.value;
            profile.NoteSpeed = Mathf.RoundToInt(noteSpeedSlider.value);
            profile.QualityIndex = qualityDropdown.value;

            var res = availableResolutions[resolutionDropdown.value];
            profile.Resolution = new ResolutionData(res.width, res.height);

            int laneCount = currentLaneCount;

            if (!profile.RebindsByLaneCount.ContainsKey(laneCount))
                profile.RebindsByLaneCount[laneCount] = new InputBindingSet();

            var set = profile.RebindsByLaneCount[laneCount];

            set.KeyboardJSON = inputController.SaveBindingGroup(InputDeviceType.Keyboard);
            set.GamepadJSON = inputController.SaveBindingGroup(InputDeviceType.Gamepad);

            ApplyToSystem(profile, res.refreshRateRatio);
            settingsManager.SaveSettingsToJSON();
        }
        private void ApplyToSystem(SettingsProfile profile, RefreshRate refreshRate)
        {
            AudioListener.volume = profile.MusicVolume;

            QualitySettings.SetQualityLevel(profile.QualityIndex);

            Screen.SetResolution(
                profile.Resolution.Width,
                profile.Resolution.Height,
                FullScreenMode.FullScreenWindow,
                refreshRate
            );
        }
        public void OnNoteSpeedChanged(float value)
        {
            noteSpeedValueText.text = Mathf.RoundToInt(value).ToString();
        }
        public void OnProfileChanged(int index)
        {
            settingsManager.SetProfileIndex(index);
            LoadCurrentProfileIntoUI();
            settingsManager.ApplyActiveProfileToSystem();
        }
        public void CreateNewProfile()
        {
            settingsManager.CreateNewProfile(new SettingsProfile(
                musicVolumeSlider.value,
                soundVolumeSlider.value,
                Mathf.RoundToInt(noteSpeedSlider.value),
                new ResolutionData(Screen.width, Screen.height),
                QualitySettings.GetQualityLevel(),
                new Dictionary<int, InputBindingSet>()
            ));

            SetupProfileDropdown();
            profileDropdown.value = settingsManager.SafeProfileIndex;
        }
        public void DeleteCurrentProfile()
        {
            settingsManager.DeleteCurrentProfile();
            SetupProfileDropdown();
            LoadCurrentProfileIntoUI();
        }
        public void RevertChanges()
        {
            LoadCurrentProfileIntoUI();
        }
        private int FindResolutionIndex(ResolutionData data)
        {
            for (int i = 0; i < availableResolutions.Length; i++)
            {
                if (availableResolutions[i].width == data.Width &&
                    availableResolutions[i].height == data.Height)
                    return i;
            }
            return 0;
        }
    }
}