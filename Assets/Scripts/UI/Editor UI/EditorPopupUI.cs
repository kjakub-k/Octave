using System.Threading.Tasks;
using UnityEngine.UIElements;
public class EditorPopupUI
{
    private VisualElement popupWindow = new();
    private TextField inputField;
    private Button acceptBtn;
    private TaskCompletionSource<string> popupTcs;
    public EditorPopupUI(VisualElement root)
    {
        popupWindow.name = "Popup";

        acceptBtn.clicked += () => OnAcceptButtonPress();

        popupWindow.Add(inputField);
        popupWindow.Add(acceptBtn);
        root.Add(popupWindow);
    }
    private void OnAcceptButtonPress()
    {
        if (popupTcs != null && !popupTcs.Task.IsCompleted)
        {
            string userInput = inputField.value;
            popupWindow.style.display = DisplayStyle.None;
            popupTcs.SetResult(userInput);
        }
    }
    public async Task<string> CreatePopupAsync()
    {
        inputField.value = "";
        popupWindow.style.display = DisplayStyle.Flex;

        popupTcs = new TaskCompletionSource<string>();

        return await popupTcs.Task;
    }
}