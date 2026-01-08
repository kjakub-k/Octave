using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.LevelSelect
{
    public class LevelItemUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text nameLabel;
        [SerializeField]
        private Image background;
        public void UpdateSelection()
        {
            background.color = Color.green;
        }
        public void NotSelected()
        {
            background.color = Color.orange;
        }
    }
}