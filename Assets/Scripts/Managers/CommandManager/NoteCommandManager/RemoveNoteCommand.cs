using System.Collections.Generic;
using UnityEngine.UIElements;
using KJakub.Octave.Data;
using System.Linq;
namespace KJakub.Octave.Managers.CommandManager
{
    public class RemoveNoteCommand : Command
    {
        private NoteData note;
        private List<NoteData> notes;
        private VisualElement visual;
        public RemoveNoteCommand(NoteData note, List<NoteData> notes, VisualElement visual)
        {
            this.note = note;
            this.notes = notes;
            this.visual = visual;
        }
        public override void Execute()
        {
            //doing this since loaded notes from a file in editor do not remember references
            //"first" because there is only ever one note in the list
            var noteToRemove = notes.Where(n => n.Lane == note.Lane && n.Time == note.Time).First();

            notes.Remove(noteToRemove);
            visual.RemoveFromClassList("classic");
        }
        public override void Undo()
        {
            notes.Add(note);
            visual.AddToClassList("classic");
        }
    }
}