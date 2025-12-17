using DG.Tweening;
using TMPro;
using UnityEngine;
namespace KJakub.Octave.UI.Game
{
    public class LatestAccuracyUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text titleLabel;
        [Header("Properties")]
        [SerializeField] private float scaleStrength = 0.2f;
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private float moveDistance = 20f;
        [SerializeField] private float animDuration = 0.3f;
        [SerializeField] private float lifeDuration = 1f;

        private Vector3 startScale;
        private Vector3 startPos;
        private Sequence sequence;

        private void Awake()
        {
            startScale = titleLabel.rectTransform.localScale;
            startPos = titleLabel.rectTransform.anchoredPosition;
        }

        public void ShowLabel(string title, Color color)
        {
            sequence?.Kill();

            titleLabel.text = title;
            titleLabel.color = color;

            var rect = titleLabel.rectTransform;
            rect.localScale = startScale;
            rect.anchoredPosition = startPos;
            titleLabel.alpha = 0f;

            sequence = DOTween.Sequence()
                .SetTarget(titleLabel)
                .Append(titleLabel.DOFade(1f, fadeDuration))
                .Join(rect.DOAnchorPosY(startPos.y + moveDistance, animDuration))
                .Join(rect.DOScale(startScale * (1f + scaleStrength), animDuration))
                .AppendInterval(lifeDuration)
                .Append(rect.DOAnchorPos(startPos, animDuration))
                .Join(rect.DOScale(startScale, animDuration))
                .Join(titleLabel.DOFade(0f, fadeDuration))
                .SetEase(Ease.OutCubic);
        }
    }
}