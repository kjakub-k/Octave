using UnityEngine;
using UnityEngine.UIElements;

public class EditorUI : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset editorLayout;
    private CommandManager cmdManager = new();
    private SongData currentSongData = new();
    private VisualElement root;
    private NavbarUI navbar;
    private TimelineUI timeline;
    private AudioWaveformUI waveform;
    private EditorPopupUI popup;
    private void Start()
    {
        SwitchTo();
    }
    public void SwitchTo()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        editorLayout.CloneTree(root);
        popup = new(root);
        navbar = new(root, cmdManager, popup, currentSongData);
    }
}
