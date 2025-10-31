using KJakub.Octave.Game.Spawning;
using UnityEngine;
namespace KJakub.Octave.Game.Lines
{
    public class NoteDetector : MonoBehaviour
    {
        public INoteCollection NoteCollection { get; set; }
        public void OnNoteDetectorPress()
        {
            foreach (var note in NoteCollection.ActiveNotes)
            {
                float detectionRadius = 0.5f;

                if (Vector3.Distance(note.transform.position, transform.position) <= detectionRadius)
                {
                    NoteCollection.ActiveNotes.Remove(note);
                    NoteCollection.NotePool.Pool.Release(note);
                }
            }
        }
    }
}