using KJakub.Octave.Data;
using KJakub.Octave.Editor.Interfaces;
using KJakub.Octave.Editor.Logic;
using UnityEngine.UIElements;
namespace KJakub.Octave.Editor.UI
{
    public class SaveUI : IEditorSave
    {
        private VisualElement saveWindow;
        private TextField songNameField;
        private TextField authorField;
        private TextField mapperField;
        private Button saveBtn;
        private Button exitBtn;
        public SaveUI(VisualElement root, SaveLogic logic, SongData songData)
        {
            saveWindow = root.Q<VisualElement>("SaveWindow");
            songNameField = root.Q<TextField>("SongNameTextField");
            authorField = root.Q<TextField>("AuthorTextField");
            mapperField = root.Q<TextField>("MapperTextField");
            saveBtn = root.Q<Button>("SaveBtn");
            exitBtn = root.Q<Button>("ExitSaveWindowBtn");

            saveBtn.clicked += () => 
            {
                logic.Save(songData, new(songNameField.value, authorField.value, mapperField.value, songData.BPM, songData.Lines, songData.Snapping));
                Hide();  
            };
            exitBtn.clicked += () => Hide();

            Hide();
        }
        public void Show()
        {
            saveWindow.AddToClassList("open");
            saveWindow.RemoveFromClassList("closed");
        }
        public void Hide()
        {
            songNameField.value = "";
            authorField.value = "";
            mapperField.value = "";
            saveWindow.AddToClassList("closed");
            saveWindow.RemoveFromClassList("open");
        }
    }
}