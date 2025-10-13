using System;
using UnityEngine.UIElements;
using System.Collections.Generic;
[UxmlElement]
partial class DropdownButton : VisualElement
{
    private VisualElement container;
    private Button toggleButton;
    [UxmlAttribute]
    public string ToggleButtonText { get { return toggleButton.text; } set { toggleButton.text = value; } }
    public DropdownButton()
    {
        CreateBody("Menu");
    }
    public DropdownButton(List<(string, Action)> buttons)
    {
        CreateBody("Menu");

        foreach (var button in buttons)
        {
            AddOption(button.Item1, button.Item2);
        }
    }
    public DropdownButton(string toggleButtonName, List<(string, Action)> buttons)
    {
        CreateBody(toggleButtonName);

        foreach (var button in buttons)
        {
            AddOption(button.Item1, button.Item2);
        }
    }
    private void CreateBody(string toggleButtonName)
    {
        AddToClassList("dropdown-button-root");

        toggleButton = new(() => ToggleDropdown())
        {
            text = toggleButtonName
        };

        toggleButton.AddToClassList("dropdown-toggle-button");

        Add(toggleButton);

        container = new();
        container.AddToClassList("dropdown-container");
        container.style.position = Position.Absolute;
        container.style.top = Length.Percent(100);
        container.style.width = Length.Percent(100);
        container.style.display = DisplayStyle.None;

        Add(container);

        //I want to register callback on root since I need to know whether or not the user had clicked
        //outside or inside the dropdown menus (I want to close them when outside)
        RegisterCallback<AttachToPanelEvent>(evt =>
        {
            var root = panel.visualTree;

            if (root != null)
            {
                root.RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);
            }
        });
    }
    private void ToggleDropdown()
    {
        container.style.display = (container.style.display == DisplayStyle.None) ? DisplayStyle.Flex : DisplayStyle.None;
    }
    /// <summary>
    /// Adds a button to the end of the dropdown menu.
    /// </summary>
    /// <param name="btnText">The text for the button.</param>
    /// <param name="onClick">What happens after the button gets pressed.</param>
    public void AddOption(string btnText, Action onClick)
    {
        Button button = new();
        button.text = btnText;
        button.clicked += onClick;
        button.clicked += ToggleDropdown;
        button.AddToClassList("dropdown-button");
        container.Add(button);
    }
    private void OnMouseDown(MouseDownEvent evt)
    {
        if (container.style.display == DisplayStyle.None)
            return;

        if (!Contains(evt.target as VisualElement))
            container.style.display = DisplayStyle.None;
    }
}