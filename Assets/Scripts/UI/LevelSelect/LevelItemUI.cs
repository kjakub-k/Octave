using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace KJakub.Octave.UI.LevelSelect
{
    public class LevelItemUI : MonoBehaviour, IPointerClickHandler
    {
        [Header("Components")]
        [SerializeField]
        private TMP_Text nameLabel;
        [SerializeField]
        private Image background;
        [Header("Scale")]
        [SerializeField]
        private float hoverScale = 1.15f;
        [SerializeField]
        private float scaleDuration = 0.1f;
        [Header("Color")]
        [SerializeField]
        private Color hoverColor = Color.white;
        [SerializeField]
        private float colorDuration = 0.1f;
        [Header("Text")]
        [SerializeField]
        private Color textHoverColor = Color.white;
        [Header("Ease")]
        [SerializeField]
        private Ease ease = Ease.OutBack;
        private Vector3 startScale;
        private Color startTextColor;
        private Color startColor;
        private Tween scaleTween;
        private Tween colorTween;
        private int indexToGive = 0;
        private LevelSelectUI levelSelectUI;
        public int IndexToGive { get { return indexToGive; } set { indexToGive = value; } }
        public LevelSelectUI LevelSelectUI { get { return levelSelectUI; } set { levelSelectUI = value; } }
        private void Awake()
        {
            startScale = transform.localScale;
            startColor = background.color;
            startTextColor = nameLabel.color;
        }
        public void UpdateSelection()
        {
            OnHovered();
        }
        public void NotSelected()
        {
            OnUnhovered();
        }
        private void OnHovered()
        {
            scaleTween?.Kill();
            colorTween?.Kill();

            scaleTween = transform
                .DOScale(hoverScale, scaleDuration)
                .SetEase(ease);

            colorTween = background
                .DOColor(hoverColor, colorDuration);

            nameLabel.color = textHoverColor;
        }
        private void OnUnhovered()
        {
            scaleTween?.Kill();
            colorTween?.Kill();

            scaleTween = transform
                .DOScale(startScale, scaleDuration)
                .SetEase(ease);

            colorTween = background
                .DOColor(startColor, colorDuration);

            nameLabel.color = startTextColor;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            levelSelectUI.OnLevelPressed(indexToGive);
        }
    }
}