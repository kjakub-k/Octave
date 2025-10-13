using UnityEngine;
using UnityEngine.UIElements;

public class EditorUI : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset editorLayout;
    private CommandManager cmdManager = new();
    private SongData currentSongData = new();
    private NavbarUI navbar;
    private TimelineUI timeline;
    private AudioWaveformUI waveform;
    private EditorPopupUI popup;
    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        SwitchTo(root);
    }
    /// <summary>
    /// Changes the root to the editor's UI hierarchy.
    /// </summary>
    //TODO: this should probably be an interface or a class
    public void SwitchTo(VisualElement root)
    {
        root.Clear();
        editorLayout.CloneTree(root);
        popup = new(root);
        navbar = new(root, cmdManager, popup, currentSongData);
    }
}
