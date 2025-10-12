using System;
using UnityEngine.UIElements;
using System.Collections.Generic;
[UxmlElement]
partial class DropdownButton : VisualElement
{
    private VisualElement container;
    private VisualElement toggleButton;
    public DropdownButton()
    {
        CreateBody("Menu");
    }
    public DropdownButton(List<(string, Action)> buttons)
    {
        CreateBody("Menu");

        foreach (var button in buttons)
        {
            AddButton(button.Item1, button.Item2);
        }
    }
    public DropdownButton(string toggleButtonName, List<(string, Action)> buttons)
    {
        CreateBody(toggleButtonName);

        foreach (var button in buttons)
        {
            AddButton(button.Item1, button.Item2);
        }
    }
    private void CreateBody(string toggleButtonName)
    {
        AddToClassList("dropdown-button-root");

        toggleButton = new Button(() => ToggleDropdown())
        {
            text = toggleButtonName
        };

        toggleButton.AddToClassList("dropdown-toggle-button");

        Add(toggleButton);

        container = new VisualElement();
        container.AddToClassList("dropdown-container");
        container.style.position = Position.Absolute;
        container.style.top = Length.Percent(100);
        container.style.width = Length.Percent(100);
        container.style.display = DisplayStyle.None;

        Add(container);
    }
    private void ToggleDropdown()
    {
        container.style.display = container.style.display == DisplayStyle.None
            ? DisplayStyle.Flex
            : DisplayStyle.None;
    }
    public void AddButton(string label, Action onClick)
    {
        var button = new Button(onClick)
        {
            text = label
        };

        button.AddToClassList("dropdown-button");
        container.Add(button);
    }
}