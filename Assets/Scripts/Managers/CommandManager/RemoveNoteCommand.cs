using System.Collections.Generic;
using UnityEngine.UIElements;
using KJakub.Octave.Data;
namespace KJakub.Octave.Managers.CommandManager
{
    public class RemoveNoteCommand : Command
    {
        private NoteData note;
        private List<NoteData> notes;
        private VisualElement visual;
        private VisualElement container;
        public RemoveNoteCommand(NoteData note, List<NoteData> notes, VisualElement visual, VisualElement container)
        {
            this.note = note;
            this.notes = notes;
            this.visual = visual;
            this.container = container;
        }
        public override void Execute()
        {
            notes.Remove(note);
            if (visual != null && visual.parent == container)
                container.Remove(visual);
        }
        public override void Undo()
        {
            notes.Add(note);
            if (visual != null && visual.parent == null)
                container.Add(visual);
        }
    }
}