using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
namespace KJakub.Octave.UI.AlbumSelect
{
    public class AlbumSelectButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        [Header("Ease")]
        [SerializeField]
        private Ease ease = Ease.OutBack;
        private Vector3 startScale;
        private Color startColor;
        private Image image;
        private Tween scaleTween;
        private Tween colorTween;
        void Awake()
        {
            image = GetComponent<Image>();
            startScale = transform.localScale;
            startColor = image.color;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            scaleTween?.Kill();
            colorTween?.Kill();

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
    }
}