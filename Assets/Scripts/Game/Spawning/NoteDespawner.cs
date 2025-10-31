using UnityEngine;
namespace KJakub.Octave.Game.Spawning
{
    public class NoteDespawner
    {
        private INoteCollection noteCollection;
        public NoteDespawner(INoteCollection noteCollection)
        {
            this.noteCollection = noteCollection;
        }
        public void CheckIfOutOfBounds()
        {
            for (int i = 0; i < noteCollection.ActiveNotes.Count; i++)
            {
                var note = noteCollection.ActiveNotes[i];

                if (note.transform.position.z > 0)
                    DespawnNote(note);
            }
        }
        public void DespawnNote(GameObject note)
        {
            noteCollection.NotePool.Pool.Release(note);
            noteCollection.ActiveNotes.Remove(note);
        }
    }
}