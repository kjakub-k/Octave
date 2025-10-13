using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class NavbarUI
{
    private VisualElement navbar;
    public NavbarUI(VisualElement root, CommandManager cmdManager, EditorPopupUI popup, SongData songData)
    {
        navbar = root.Q<VisualElement>("Navbar");

        AddOptionsToDropdownMenu("File", new() { 
            ("Save File", () => { Debug.Log("Ulozeno"); }), 
            ("Load File", () => { Debug.Log("Nacteno"); }) 
        });
        AddOptionsToDropdownMenu("Edit", new() { 
            ("Undo", () => { cmdManager.Undo(); }), 
            ("Redo", () => { cmdManager.Redo(); }) 
        });
        AddOptionsToDropdownMenu("Song", new() { 
            ("Set Song", async () => { songData.Song = await popup.CreatePopupAsync<AudioClip>(); }),
            ("Set BPM", async () => { songData.BPM = await popup.CreatePopupAsync<int>(); }),
            ("Set Lines Amount", async () => { songData.Lines = await popup.CreatePopupAsync<int>(); }),
            ("Set Snapping", async () => { songData.Snapping = await popup.CreatePopupAsync<SnappingType>(); })
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