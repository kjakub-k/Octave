using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Game
{
    public class ThermometerUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private Image fill;
        public void UpdateValue(float value)
        {
            fill.fillAmount = value / 100;
        }
    }
}