using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Achievements
{
    public class AchievementItemUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private Image background;
        [SerializeField]
        private Shadow shadow;
        [SerializeField]
        private TMP_Text titleLabel;
        [SerializeField]
        private TMP_Text descriptionLabel;
        [SerializeField]
        private Image iconImage;
        [Header("Color Settings")]
        [SerializeField]
        private Color unlockedColorBg;
        [SerializeField]
        private Color unlockedColorShadow;
        [SerializeField]
        private Color lockedColorBg;
        [SerializeField]
        private Color lockedColorShadow;
        public void UpdateInfo(string title, string description, Sprite icon, Color backgroundColor, Color shadowColor)
        {
            titleLabel.text = title;
            descriptionLabel.text = description;
            iconImage.sprite = icon;
            unlockedColorBg = backgroundColor;
            unlockedColorShadow = shadowColor;
        }
        public void Unlock()
        {
            background.color = unlockedColorBg;
            shadow.effectColor = unlockedColorShadow;
        }
        public void Lock()
        {
            background.color = lockedColorBg;
            shadow.effectColor = lockedColorShadow;
        }
    }
}