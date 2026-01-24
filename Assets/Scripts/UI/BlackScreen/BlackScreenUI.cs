using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.BlackScreen 
{ 
    public class BlackScreenUI : MonoBehaviour 
    {
        [Header("Components")]
        [SerializeField]
        private RawImage background;
        [Header("Properties")]
        private float fadeDuration = 0.3f;
        private Tween showTween;
        public Tween Show()
        {
            showTween?.Kill();

            showTween = background
                .DOFade(1, fadeDuration)
                .SetEase(Ease.OutBack);

            return showTween;
        }
        public Tween Hide()
        {
            showTween?.Kill();

            showTween = background
                .DOFade(0, fadeDuration)
                .SetEase(Ease.OutBack);
            
            return showTween;
        }
    }
}