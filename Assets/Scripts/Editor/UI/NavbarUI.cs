using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using KJakub.Octave.Editor.Logic;
using KJakub.Octave.CustomElements;
namespace KJakub.Octave.Editor.UI
{
    public class NavbarUI
    {
        private VisualElement navbar;
        public NavbarUI(VisualElement root, NavbarLogic navbarLogic)
        {
            navbar = root.Q<VisualElement>("Navbar");

            AddOptionsToDropdownMenu("File", new() { 
                ("Save File", navbarLogic.Save), 
                ("Load File", navbarLogic.Load)
            });
            AddOptionsToDropdownMenu("Edit", new() { 
                ("Undo", navbarLogic.Undo), 
                ("Redo", navbarLogic.Redo) 
            });
            AddOptionsToDropdownMenu("Song", new() {
                //Discard the returned Task since Action expects a synchronous delegate
                //and we don�t need to await the async method here
                ("Set Song", () => _ = navbarLogic.SetSong()),
                ("Set BPM", () => _ = navbarLogic.SetBPM()),
                ("Set Lines Amount", () => _ = navbarLogic.SetLineAmount()),
                ("Set Snapping", () => _ = navbarLogic.SetSnapping())
            });
        }
        private void AddOptionsToDropdownMenu(string elementName, List<(string, Action)> options)
        {
            DropdownButton ddBtn = navbar.Q<DropdownButton>(elementName);
        
            foreach (var option in options)
            {
                ddBtn.AddOption(option.Item1, option.Item2);
            }
        }
    }
}