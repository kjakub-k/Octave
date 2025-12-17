using TMPro;
using UnityEngine;
using DG.Tweening;
namespace KJakub.Octave.UI.Game
{
    public class HighestComboUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private TMP_Text label;
        [Header("Properties")]
        [SerializeField]
        private float fadeDuration;
        public void ChangeText(string text)
        {
            label.text = text;
        }
        public void Show()
        {
            label.DOFade(1f, fadeDuration);
        }
        public void Hide()
        {
            label.DOFade(0f, fadeDuration);
        }
    }
}
