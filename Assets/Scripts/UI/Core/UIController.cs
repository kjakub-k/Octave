using UnityEngine;
using UnityEngine.UIElements;

namespace KJakub.Octave.UI.Core
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private UIDocument editorLayout;
        [SerializeField]
        private UIDocument gameLayout;
        private void Start()
        {
            EnableEditor();
        }
        public void EnableEditor()
        {
            editorLayout.gameObject.SetActive(true);
        }
    }
}