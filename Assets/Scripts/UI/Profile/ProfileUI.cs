using KJakub.Octave.UI.Core;
using TMPro;
using UnityEngine;
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
        private void OnEnable()
        {
            if (IsSignedIn())
            {
                UpdateScoreLabel(PlayerPrefs.GetInt("TotalScore", 0));
                UpdateRatingLabel(0);
            }
            else
            {
                UpdateScoreLabel(PlayerPrefs.GetInt("TotalScore", 0));
                UpdateRatingLabel(0);
            }
        }
        public void BackToMenuBtn()
        {
            uiController.HideProfile();
            uiController.ShowMainMenu();
        }
        public void UpdateScoreLabel(int score)
        {
            scoreLabel.text = score.ToString();
        }
        public void UpdateRatingLabel(float rating)
        {
            ratingLabel.text = $"{rating}%";
        }
        public bool IsSignedIn()
        {
            return false;
        }
    }
}