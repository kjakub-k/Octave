using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Profile
{
    public class AchievementProfileItem : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text titleLabel;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private Image background;
        [SerializeField]
        private Shadow shadow;
        public void UpdateInfo(string title, Sprite texture, Color bgColor, Color shColor)
        {
            titleLabel.text = title;
            icon.sprite = texture;
            background.color = bgColor;
            shadow.effectColor = shColor;
        }
    }
}