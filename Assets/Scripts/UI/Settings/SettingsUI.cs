using KJakub.Octave.Data;
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
        private Resolution[] availableResolutions;
        private void Start()
        {
            SetupQualityDropdown();
            SetupResolutionDropdown();
            SetupProfileDropdown();
            LoadCurrentProfileIntoUI();
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
                options.Add($"Profile {i + 1}");

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
        }
        public void CreateNewProfile()
        {
            settingsManager.CreateNewProfile(new SettingsProfile(
                musicVolumeSlider.value,
                soundVolumeSlider.value,
                Mathf.RoundToInt(noteSpeedSlider.value),
                new ResolutionData(Screen.width, Screen.height),
                QualitySettings.GetQualityLevel(),
                string.Empty
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