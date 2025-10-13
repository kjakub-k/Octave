using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class EditorPopupUI
{
    private readonly VisualElement popupWindow = new();
    private TextField textInputField;
    private EnumField enumField;
    private Button acceptBtn = new() { text = "Accept" };
    private TaskCompletionSource<object> popupTcs;
    public EditorPopupUI(VisualElement root)
    {
        popupWindow.name = "Popup";
        popupWindow.style.display = DisplayStyle.None;

        acceptBtn.clicked += OnAcceptButtonPress;

        popupWindow.Add(acceptBtn);
        root.Add(popupWindow);
    }
    private void OnAcceptButtonPress()
    {
        if (popupTcs == null || popupTcs.Task.IsCompleted)
            return;

        object result = null;

        if (textInputField != null)
        {
            result = textInputField.value;
        }
        else if (enumField != null)
        {
            result = enumField.value;
        }

        popupWindow.style.display = DisplayStyle.None;
        popupWindow.Clear();
        popupWindow.Add(acceptBtn);

        popupTcs.SetResult(result);
    }

    public async Task<T> CreatePopupAsync<T>()
    {
        popupTcs = new TaskCompletionSource<object>();

        Type type = typeof(T);
        object defaultValue = default(T);

        popupWindow.style.display = DisplayStyle.Flex;

        if (type == typeof(int))
        {
            textInputField = new TextField("Enter an integer:");
            textInputField.value = "0";
            popupWindow.Insert(0, textInputField);
        }
        else if (type.IsEnum)
        {
            enumField = new EnumField("Choose:", (Enum)Enum.GetValues(type).GetValue(0));
            popupWindow.Insert(0, enumField);
        }
        else
        {
            throw new NotSupportedException($"Type {typeof(T).Name} not supported");
        }

        object result = await popupTcs.Task;
        return (T)Convert.ChangeType(result, typeof(T));
    }
}