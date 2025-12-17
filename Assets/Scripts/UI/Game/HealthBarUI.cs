using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Game
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField]
        private Image fill;
        public void UpdateHealth(int amount, int max)
        {
            fill.DOFillAmount(amount / (float)max, 0.3f);
        }
        public void ChangeFillColor(Color newColor, float duration)
        {
            fill.DOColor(newColor, duration);
        }
    }
}