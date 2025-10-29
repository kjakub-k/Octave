using KJakub.Octave.Editor.Interfaces;
using System.Threading.Tasks;
using UnityEngine.UIElements;
namespace KJakub.Octave.Editor.UI
{
    public class EditorPopupUI : IEditorPopup
    {
        private VisualElement popupWindow;
        private TextField textInputField;
        private EnumField enumField;
        private Button cancelBtn;
        private Button acceptBtn;
        private TaskCompletionSource<object> popupTcs;
        public EditorPopupUI(VisualElement root)
        {
            popupWindow = root.Q<VisualElement>("Popup");

            PopupClose();

            textInputField = popupWindow.Q<TextField>("EditorPopupTextField");
            enumField = popupWindow.Q<EnumField>("EditorPopupEnumField");
            cancelBtn = popupWindow.Q<Button>("EditorPopupCancelBtn");
            acceptBtn = popupWindow.Q<Button>("EditorPopupAcceptBtn");

            acceptBtn.clicked += OnAcceptButtonPress;
            cancelBtn.clicked += OnCancelButtonPress;
        }
        private void OnAcceptButtonPress()
        {
            object result = null;

            if (textInputField.style.display == DisplayStyle.Flex)
                result = textInputField.value;
            else if (enumField.style.display == DisplayStyle.Flex)
                result = enumField.value;

            PopupClose();
            textInputField.value = string.Empty;
            enumField.value = null;

            popupTcs.SetResult(result);
        }
        private void PopupClose()
        {
            popupWindow.RemoveFromClassList("open");
            popupWindow.AddToClassList("closed");
        }
        private void PopupOpen()
        {
            popupWindow.RemoveFromClassList("closed");
            popupWindow.AddToClassList("open");
        }
        private void OnCancelButtonPress()
        {
            PopupClose();
            textInputField.value = string.Empty;
            enumField.value = null;

            popupTcs.SetResult(null);
        }
        private async Task<SnappingType?> CreateSnappingTypePopup()
        {
            popupTcs = new TaskCompletionSource<object>();

            PopupOpen();
            enumField.style.display = DisplayStyle.Flex;
            textInputField.style.display = DisplayStyle.None;

            object result = await popupTcs.Task;

            if (result == null)
                return null;

            return (SnappingType)result;
        }
        private async Task<int?> CreateIntPopup()
        {
            popupTcs = new TaskCompletionSource<object>();

            PopupOpen();
            textInputField.style.display = DisplayStyle.Flex;
            enumField.style.display = DisplayStyle.None;

            object result = await popupTcs.Task;

            if (result == null)
                return null;

            if (int.TryParse(result.ToString(), out int intResult))
                return intResult;

            return null;
        }
        public Task<int?> RequestInt()
        {
            return CreateIntPopup();
        }
        public Task<SnappingType?> RequestSnappingType()
        {
            return CreateSnappingTypePopup();
        }
    }
}