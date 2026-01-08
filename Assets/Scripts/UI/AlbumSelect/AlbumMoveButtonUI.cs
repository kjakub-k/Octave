using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace KJakub.Octave.UI.AlbumSelect
{
    public class AlbumMoveButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Colors")]
        [SerializeField] 
        private Color disabledColor = Color.gray;
        [SerializeField]
        private Color hoverColor = Color.white;
        [Header("Properties")]
        [SerializeField] 
        private float colorChangeDuration = 0.3f;
        private bool isCorrect;
        private Color defaultColor;
        private TMP_Text buttonText;
        private Tween colorTween;
        private void Awake()
        {
            buttonText = GetComponentInChildren<TMP_Text>();
            defaultColor = buttonText.color;
        }
        public void UpdateSelection(bool isCorrect)
        {
            this.isCorrect = isCorrect;

            if (!isCorrect)
                buttonText.color = disabledColor;
            else
                buttonText.color = defaultColor;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            AnimateColor(hoverColor);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            AnimateColor(defaultColor);
        }
        private void AnimateColor(Color targetColor)
        {
            if (!isCorrect)
                return;

            if (colorTween != null && colorTween.IsActive())
                colorTween.Kill();

            colorTween = buttonText.DOColor(targetColor, colorChangeDuration).SetEase(Ease.Linear);
        }
    }
}