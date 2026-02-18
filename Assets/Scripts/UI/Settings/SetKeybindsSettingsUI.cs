using UnityEngine;
namespace KJakub.Octave.UI.Settings
{
    public class SetKeybindsSettingsUI : MonoBehaviour
    {
        [SerializeField] private GameObject scrollView;
        [SerializeField] private GameObject otherScrollView;
        public void Open()
        {
            scrollView.SetActive(true);
            otherScrollView.SetActive(false);
        }
        public void Close()
        {
            scrollView.SetActive(false);
            otherScrollView.SetActive(true);
        }
    }
}