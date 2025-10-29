using System;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.CustomElements 
{ 
    [UxmlElement]
    public partial class DropdownButton : VisualElement
    {
        private VisualElement container;
        private VisualElement higherLevelContainer; //where the container with buttons are so they can be clicked and not overlayed by elements below toggle button
        private Button toggleButton;
        private Button secondToggleButton;
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
            ToggleButtonText = toggleButtonName;
            CreateBody(toggleButtonName);

            foreach (var button in buttons)
            {
                AddOption(button.Item1, button.Item2);
            }
        }
        private void CreateBody(string toggleButtonText)
        {
            AddToClassList("dropdown-button-root");

            toggleButton = new(() => ToggleDropdown())
            {
                text = toggleButtonText
            };

            toggleButton.AddToClassList("dropdown-toggle-button");
            toggleButton.AddToClassList("menu-up");

            container = new();
            container.AddToClassList("dropdown-container");
            container.AddToClassList("menu-up");
            container.style.position = Position.Absolute;

            Add(toggleButton);

            //I want to register callback on root (panel) since I need to know whether or not the user had clicked
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
        private void CreateDropdownMenu()
        {
            //need to put the container in a higher level one so the buttons are actually clickable
            //(they are overlayed by whatever is underneath the button)
            higherLevelContainer = new();
            higherLevelContainer.style.position = Position.Absolute;
            higherLevelContainer.style.height = toggleButton.style.height;
            higherLevelContainer.style.width = toggleButton.style.width;

            //getting the root like this because unity does not have a built in function to get the UI document root
            VisualElement root = this;

            while (root.parent != null && root.parent.parent != null)
                root = root.parent;

            root.Add(higherLevelContainer);

            var buttonWorldPos = toggleButton.LocalToWorld(Vector2.zero);
            var rootPos = root.WorldToLocal(buttonWorldPos);

            higherLevelContainer.style.left = rootPos.x;
            higherLevelContainer.style.top = rootPos.y;
            higherLevelContainer.style.width = toggleButton.resolvedStyle.width;
            higherLevelContainer.style.height = toggleButton.resolvedStyle.height;

            container.style.top = rootPos.y + toggleButton.resolvedStyle.height;
            container.style.width = toggleButton.resolvedStyle.width;

            secondToggleButton = new(() => ToggleDropdown())
            {
                text = toggleButton.text
            };
            secondToggleButton.AddToClassList("dropdown-toggle-button");
            secondToggleButton.AddToClassList("menu-up");

            toggleButton.SetEnabled(false);

            higherLevelContainer.Add(container);
            higherLevelContainer.Add(secondToggleButton);

        }
        private void ToggleDropdown()
        {
            if (higherLevelContainer == null)
            {
                CreateDropdownMenu();
            }

            if (container.ClassListContains("menu-up"))
            {
                container.RemoveFromClassList("menu-up");
                container.AddToClassList("menu-down");
                secondToggleButton.RemoveFromClassList("menu-up");
                secondToggleButton.AddToClassList("menu-down");
            }
            else
            {
                container.RemoveFromClassList("menu-down");
                container.AddToClassList("menu-up");
                secondToggleButton.RemoveFromClassList("menu-down");
                secondToggleButton.AddToClassList("menu-up");
            }
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
            if (container.ClassListContains("menu-up"))
                return;

            var targetElement = evt.target as VisualElement;

            if (Contains(targetElement) || (higherLevelContainer != null && higherLevelContainer.Contains(targetElement)))
                return;

            container.RemoveFromClassList("menu-down");
            container.AddToClassList("menu-up");
            secondToggleButton.RemoveFromClassList("menu-down");
            secondToggleButton.AddToClassList("menu-up");
        }
    }
}