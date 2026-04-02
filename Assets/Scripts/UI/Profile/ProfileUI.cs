using KJakub.Octave.UI.Core;
using UnityEngine;
namespace KJakub.Octave.UI.Profile
{
    public class ProfileUI : MonoBehaviour
    {
        [SerializeField]
        private UIController uiController;
        public void BackToMenuBtn()
        {
            uiController.HideProfile();
            uiController.ShowMainMenu();
        }
    }
}