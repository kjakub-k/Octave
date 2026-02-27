using KJakub.Octave.Data;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Profiling;
namespace KJakub.Octave.Managers.SettingsManager
{
    public class SettingsManager : MonoBehaviour
    {
        public List<SettingsProfile> Profiles { get; private set; } = new();

        private string SavePath =>
            Path.Combine(Application.persistentDataPath, "settings_profiles.json");

        public int SafeProfileIndex =>
            Mathf.Clamp(PlayerPrefs.GetInt("ProfileIndex", 0), 0, Profiles.Count - 1);

        public SettingsProfile CurrentProfile => Profiles[SafeProfileIndex];

        private void Awake()
        {
            Profiles = LoadAllProfiles();

            if (Profiles.Count == 0)
            {
                CreateDefaultProfile();
                PlayerPrefs.SetInt("ProfileIndex", 0);
                SaveSettingsToJSON();
            }
        }

        public void SetProfileIndex(int index)
        {
            PlayerPrefs.SetInt("ProfileIndex",
                Mathf.Clamp(index, 0, Profiles.Count - 1));
        }

        private List<SettingsProfile> LoadAllProfiles()
        {
            if (!File.Exists(SavePath))
                return new List<SettingsProfile>();

            try
            {
                string json = File.ReadAllText(SavePath);
                var profiles = JsonConvert.DeserializeObject<List<SettingsProfile>>(json)
                               ?? new List<SettingsProfile>();

                foreach (var profile in profiles)
                    profile.EnsureInitialized();

                return profiles;
            }
            catch
            {
                return new List<SettingsProfile>();
            }
        }

        public void CreateNewProfile(SettingsProfile newProfile)
        {
            Profiles.Add(newProfile);
            PlayerPrefs.SetInt("ProfileIndex", Profiles.Count - 1);
            SaveSettingsToJSON();
        }

        public void DeleteCurrentProfile()
        {
            Profiles.Remove(CurrentProfile);

            if (Profiles.Count == 0)
                CreateDefaultProfile();

            PlayerPrefs.SetInt("ProfileIndex",
                Mathf.Clamp(PlayerPrefs.GetInt("ProfileIndex"), 0, Profiles.Count - 1));

            SaveSettingsToJSON();
        }

        public void SaveSettingsToJSON()
        {
            File.WriteAllText(
                SavePath,
                JsonConvert.SerializeObject(Profiles, Formatting.Indented)
            );
        }

        private void CreateDefaultProfile()
        {
            var profile = new SettingsProfile(
                1f,
                1f,
                10,
                new ResolutionData(1920, 1080),
                QualitySettings.GetQualityLevel(),
                new Dictionary<int, string>()
            );
            profile.EnsureInitialized();
            Profiles.Add(profile);
        }
    }
}