using System;
using UnityEngine;
namespace KJakub.Octave.Game.Spawning
{
    public class NoteDespawner : MonoBehaviour
    {
        public event Action OnNoteOutOfBounds;
        public void CheckIfOutOfBounds(NoteRuntimeCollection noteCollection)
        {
            for (int i = 0; i < noteCollection.ActiveNotes.Count; i++)
            {
                var note = noteCollection.ActiveNotes[i];

                if (note.transform.position.z >= transform.position.z)
                {
                    DespawnNote(note, noteCollection);
                    OnNoteOutOfBounds?.Invoke();
                }
            }
        }
        private void DespawnNote(GameObject note, NoteRuntimeCollection noteCollection)
        {
            noteCollection.NotePool.Pool.Release(note);
            noteCollection.ActiveNotes.Remove(note);
        }
        public void DespawnAllNotes(NoteRuntimeCollection noteCollection)
        {
            foreach (var note in noteCollection.ActiveNotes)
            {
                noteCollection.NotePool.Pool.Release(note);
            }

            noteCollection.ActiveNotes.Clear();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}