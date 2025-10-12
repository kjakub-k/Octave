using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class NavbarUI
{
    private CommandManager cmdManager;
    private VisualElement navbar = new();
    public NavbarUI(VisualElement root, CommandManager cmdManager)
    {
        this.cmdManager = cmdManager;
        CreateFilesDropdownMenu();
        CreateEditDropdownMenu();
        CreateSongDropdownMenu();
        root.Q<VisualElement>("Background").Add(navbar);
    }
    private void CreateFilesDropdownMenu()
    {
        List<(string, Action)> menu = new()
        {
            ("Save File", () => { Debug.Log("Ulozeno"); }),
            ("Load File", () => { Debug.Log("Nacteno"); })
        };

        DropdownButton ddBtn = new DropdownButton("Files", menu);
        navbar.Add(ddBtn);
    }
    private void CreateEditDropdownMenu()
    {
        List<(string, Action)> menu = new()
        {
            ("Undo", () => { cmdManager.Undo(); }),
            ("Redo", () => { cmdManager.Redo(); })
        };

        DropdownButton ddBtn = new DropdownButton("Edit", menu);
        navbar.Add(ddBtn);
    }
    private void CreateSongDropdownMenu()
    {
        List<(string, Action)> menu = new()
        {
            ("Set Song", () => { }),
            ("Set BPM", () => { }),
            ("Set Lines Amount", () => { }),
            ("Set Snapping", () => { })
        };

        DropdownButton ddBtn = new DropdownButton("Song", menu);
        navbar.Add(ddBtn);
    }
}