using KJakub.Octave.Managers.GamejoltManager;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
namespace KJakub.Octave.UI.Profile 
{ 
    public class GamejoltLoginUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject loginUI;
        [SerializeField]
        private TMP_InputField inputNameField;
        [SerializeField]
        private TMP_InputField tokenField;
        [SerializeField]
        private TMP_Text errorLabel;
        [SerializeField]
        private ProfileUI profileUI;
        public void LogIn()
        {
            Login();
        }
        public async void Login()
        {
            bool success = await GamejoltLoader.Instance.Login(inputNameField.text, tokenField.text);

            if (success)
            {
                Hide();
                profileUI.UpdateUI();
                errorLabel.text = "";
            } else
            {
                errorLabel.text = "Wrong username and/or token.";
            }
        }
        public void Show()
        {
            loginUI.SetActive(true);
        }
        public void Hide()
        {
            loginUI.SetActive(false);
        }
    }
}