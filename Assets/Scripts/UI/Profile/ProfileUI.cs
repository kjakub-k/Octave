using KJakub.Octave.Managers.AchievementsManager;
using KJakub.Octave.Managers.GamejoltManager;
using KJakub.Octave.ScriptableObjects;
using KJakub.Octave.UI.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Profile
{
    public class ProfileUI : MonoBehaviour
    {
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private TMP_Text scoreLabel;
        [SerializeField]
        private TMP_Text ratingLabel;
        [SerializeField]
        private Button gamejoltLoginButton;
        [SerializeField]
        private TMP_Text gamejoltLoginButtonText;
        [SerializeField]
        private TMP_Text userLabel;
        [SerializeField]
        private AchievementsManager achievementsManager;
        [SerializeField]
        private Transform container;
        [SerializeField]
        private GameObject achievementProfileItemPrefab;
        [SerializeField]
        private GamejoltLoginUI gamejoltLoginUI;
        private int currentSlotIndex = 0;
        private string[] achievementSlots = new string[3];
        private int ignore;
        async private void OnEnable()
        {
            if (ignore <= 0)
            {
                ignore++;
                return;
            }

            if (IsSignedIn())
            {
                SetButtonAndLabelToLoggedIn();
                UpdateLabelsGamejolt();
            }
            else
            {
                Logout();
            }
        }
        async private void UpdateLabelsGamejolt()
        {
            UpdateScoreLabel("Loading...");
            UpdateRatingLabel("Loading...");
            UpdateScoreLabel(await GamejoltLoader.Instance.GetUserData("score") ?? "0");
            UpdateRatingLabel(await GamejoltLoader.Instance.GetUserData("accuracy") ?? "100");
        }
        public void SetButtonAndLabelToLoggedIn()
        {
            userLabel.text = $"{PlayerPrefs.GetString("username")}";
            gamejoltLoginButtonText.text = "Logout";
            gamejoltLoginButton.onClick.RemoveAllListeners();
            gamejoltLoginButton.onClick.AddListener(Logout);
            UpdateLabelsGamejolt();
        }
        public void Logout()
        {
            userLabel.text = "Not Logged In";
            gamejoltLoginButtonText.text = "Login";
            gamejoltLoginButton.onClick.RemoveAllListeners();
            gamejoltLoginButton.onClick.AddListener(ShowLogIn);
            PlayerPrefs.SetString("username", null);
            PlayerPrefs.SetString("token", null);
            UpdateScoreLabel($"{PlayerPrefs.GetInt("TotalScore", 0)}");
            UpdateRatingLabel($"{PlayerPrefs.GetFloat("AverageAccuracy", 100)}");
        }
        public void UpdateUI()
        {
            if (IsSignedIn())
                SetButtonAndLabelToLoggedIn();
            else
                Logout();
        }
        private void ShowLogIn()
        {
            gamejoltLoginUI.Show();
        }
        public void BackToMenuBtn()
        {
            for (int i = 0; i < achievementSlots.Length; i++)
            {
                if (achievementSlots[i] == null)
                    continue;

                GamejoltLoader.Instance.SaveAchievementDisplay(achievementSlots[i], i);
            }

            uiController.HideProfile();
            uiController.ShowMainMenu();
        }
        public void EnableAchievementPicker(int slot)
        {
            currentSlotIndex = slot;
            List<AchievementSO> achievements = achievementsManager.GetUnlockedAchievements().ToList();

            List<GameObject> objectsToDelete = new();

            for (int i = 0; i < container.childCount; i++)
            {
                objectsToDelete.Add(container.GetChild(i).gameObject);
            }

            for (int i = objectsToDelete.Count - 1; i >= 0; i--)
            {
                Destroy(objectsToDelete[i]);
            }

            foreach (AchievementSO achievement in achievements)
            {
                CreateAchievementSlotItem(achievement.Title, achievement.Texture, achievement.BackgroundColor, achievement.ShadowColor);
            }
        }
        private void CreateAchievementSlotItem(string title, Sprite texture, Color bgColor, Color shColor)
        {
            GameObject obj = Instantiate(achievementProfileItemPrefab, container);
            AchievementProfileItem aPI = obj.GetComponent<AchievementProfileItem>();
            aPI.UpdateInfo(title, texture, bgColor, shColor);
        }
        public void SaveAchievementPick(string id)
        {
            achievementSlots[currentSlotIndex] = id;
        }
        public void UpdateScoreLabel(string score)
        {
            scoreLabel.text = score;
        }
        public void UpdateRatingLabel(string rating)
        {
            if (rating != "Loading...")
                ratingLabel.text = $"{rating}%";
            else
                ratingLabel.text = rating;
        }
        public bool IsSignedIn()
        {
            if (PlayerPrefs.GetString("username").Trim() != ""
                && PlayerPrefs.GetString("token").Trim() != "")
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}