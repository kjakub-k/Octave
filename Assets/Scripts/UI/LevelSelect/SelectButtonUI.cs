using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
namespace KJakub.Octave.UI.AlbumSelect
{
    public class AlbumSelectButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
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
        private Color textHoverColor = Color.white;
        [Header("Ease")]
        [SerializeField]
        private Ease ease = Ease.OutBack;
        private Vector3 startScale;
        private Color startTextColor;
        private Color startColor;
        private Image image;
        private TMP_Text text;
        private Tween scaleTween;
        private Tween colorTween;
        private void Awake()
        {
            image = GetComponent<Image>();
            startScale = transform.localScale;
            startColor = image.color;
            text = GetComponentInChildren<TMP_Text>();
            startTextColor = text.color;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            scaleTween?.Kill();
            colorTween?.Kill();
            text.color = textHoverColor;

            scaleTween = transform
                .DOScale(startScale * hoverScale, scaleDuration)
                .SetEase(ease);

            colorTween = image
                .DOColor(hoverColor, colorDuration);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            scaleTween?.Kill();
            colorTween?.Kill();
            text.color = startTextColor;

            scaleTween = transform
                .DOScale(startScale, scaleDuration)
                .SetEase(ease);

            colorTween = image
                .DOColor(startColor, colorDuration);
        }
        void OnDisable()
        {
            scaleTween?.Kill();
            colorTween?.Kill();
            transform.localScale = startScale;
            image.color = startColor;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            text.color = startTextColor;
        }
    }
}