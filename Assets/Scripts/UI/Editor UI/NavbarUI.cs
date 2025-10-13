using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class NavbarUI
{
    private CommandManager cmdManager;
    private EditorPopupUI popup;
    private SongData songData;
    private VisualElement navbar = new();
    public NavbarUI(VisualElement root, CommandManager cmdManager, EditorPopupUI popup, SongData songData)
    {
        this.cmdManager = cmdManager;
        this.popup = popup;
        this.songData = songData;
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
            new("Set Song", () => { songData.Song = null; }),
            new("Set BPM", () => { songData.BPM = 0; }),
            new("Set Lines Amount", () => { songData.Lines = 4; }),
            new("Set Snapping", () => { songData.Snapping = SnappingType.Quarter; })
        };

        DropdownButton ddBtn = new DropdownButton("Song", menu);
        navbar.Add(ddBtn);
    }
}