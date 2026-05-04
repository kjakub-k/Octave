using KJakub.Octave.Managers.AchievementsManager;
using KJakub.Octave.Managers.GamejoltManager;
using KJakub.Octave.Managers.LanguageManager;
using KJakub.Octave.ScriptableObjects;
using KJakub.Octave.UI.Core;
using System.Collections.Generic;
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
        private Button gamejoltLoginButton;
        [SerializeField]
        private AchievementsManager achievementsManager;
        [SerializeField]
        private Transform container;
        [SerializeField]
        private GameObject achievementProfileItemPrefab;
        [SerializeField]
        private GamejoltLoginUI gamejoltLoginUI;
        [Header("Labels")]
        [SerializeField]
        private TMP_Text gamejoltLoginButtonText;
        [SerializeField]
        private TMP_Text scoreLabel;
        [SerializeField]
        private TMP_Text ratingLabel;
        [SerializeField]
        private TMP_Text userLabel;
        [SerializeField]
        private TMP_Text backBtnLabel;
        [SerializeField]
        private TMP_Text scoreTitleLabel;
        [SerializeField]
        private TMP_Text ratingTitleLabel;
        [SerializeField]
        private TMP_Text loginPopupCancelLabel;
        [SerializeField]
        private TMP_Text loginPopupLoginLabel;
        [SerializeField]
        private TMP_Text loginPopupUserLabel;
        [SerializeField]
        private TMP_Text loginPopupPlaceholderLabelForName;
        [SerializeField]
        private TMP_Text loginPopupTokenLabel;
        [SerializeField]
        private TMP_Text loginPopupPlaceholderLabelForToken;
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

            Translate();

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
        private void Translate()
        {
            backBtnLabel.text = LanguageManager.GetTranslation("back_and_save_btn");
            scoreTitleLabel.text = LanguageManager.GetTranslation("overall_score");
            ratingTitleLabel.text = LanguageManager.GetTranslation("overall_rating");
            loginPopupCancelLabel.text = LanguageManager.GetTranslation("cancel");
            loginPopupLoginLabel.text = LanguageManager.GetTranslation("login");
            loginPopupPlaceholderLabelForName.text = LanguageManager.GetTranslation("input_placeholder");
            loginPopupPlaceholderLabelForToken.text = LanguageManager.GetTranslation("input_placeholder");
            loginPopupTokenLabel.text = LanguageManager.GetTranslation("token");
            loginPopupUserLabel.text = LanguageManager.GetTranslation("user");
        }
        async private void UpdateLabelsGamejolt()
        {
            UpdateScoreLabel(LanguageManager.GetTranslation("loading"));
            UpdateRatingLabel(LanguageManager.GetTranslation("loading"));
            UpdateScoreLabel(await GamejoltLoader.Instance.GetUserData("score") ?? "0");
            UpdateRatingLabel(await GamejoltLoader.Instance.GetUserData("accuracy") ?? "100");
        }
        public void SetButtonAndLabelToLoggedIn()
        {
            userLabel.text = $"{PlayerPrefs.GetString("username")}";
            gamejoltLoginButtonText.text = LanguageManager.GetTranslation("logout");
            gamejoltLoginButton.onClick.RemoveAllListeners();
            gamejoltLoginButton.onClick.AddListener(Logout);
            UpdateLabelsGamejolt();
        }
        public void Logout()
        {
            userLabel.text = LanguageManager.GetTranslation("not_logged_in_user");
            gamejoltLoginButtonText.text = LanguageManager.GetTranslation("login");
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