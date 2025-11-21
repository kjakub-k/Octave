using KJakub.Octave.Game.Spawning;
using System;
using UnityEngine;
namespace KJakub.Octave.Game.Lines
{
    public class NoteDetector : MonoBehaviour
    {
        public INoteCollection NoteCollection { get; set; }
        public event Action<float> OnNoteHit;
        public void OnNoteDetectorPress()
        {
            foreach (var note in NoteCollection.ActiveNotes)
            {
                float detectionRadius = 1f;

                if (Vector3.Distance(note.transform.position, transform.position) <= detectionRadius)
                {
                    OnNoteHit?.Invoke(Vector3.Distance(note.transform.position, transform.position));
                    NoteCollection.ActiveNotes.Remove(note);
                    NoteCollection.NotePool.Pool.Release(note);
                    return;
                }
            }
        }
    }
}