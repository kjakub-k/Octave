using System.Collections.Generic;
using UnityEngine.UIElements;
using KJakub.Octave.Data;
namespace KJakub.Octave.Managers.CommandManager 
{ 
    public class AddNoteCommand : Command
    {
        private NoteData note;
        private List<NoteData> notes;
        private VisualElement visual;
        public AddNoteCommand(NoteData note, List<NoteData> notes, VisualElement visual)
        {
            this.note = note;
            this.notes = notes;
            this.visual = visual;
        }
        public override void Execute()
        {
            notes.Add(note);
            visual.AddToClassList("classic");
        }
        public override void Undo()
        {
            notes.Remove(note);
            visual.RemoveFromClassList("classic");
        }
    }
}