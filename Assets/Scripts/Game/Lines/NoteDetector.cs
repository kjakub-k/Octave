using KJakub.Octave.Game.Spawning;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace KJakub.Octave.Game.Lines
{
    public class NoteDetector : MonoBehaviour
    {

        public float DetectionRadius { get; set; }
        public INoteCollection NoteCollection { get; set; }
        public int Lane { get; set; }
        public InputAction AssignedAction { get; set; }
        public Action<InputAction.CallbackContext> InputHandler { get; set; }
        public event Action<float> OnNoteHit;
        public void OnNoteDetectorPress()
        {
            GameObject closestNote = null;
            float closestZ = float.NegativeInfinity;

            foreach (var note in NoteCollection.ActiveNotes)
            {
                if (note.transform.position.x == transform.position.x)
                {
                    if (Vector3.Distance(note.transform.position, transform.position) <= DetectionRadius)
                    {
                        if (note.transform.position.z > closestZ)
                        {
                            closestZ = note.transform.position.z;
                            closestNote = note;
                        }
                    }
                }
            }

            if (closestNote != null)
            {
                OnNoteHit?.Invoke(Vector3.Distance(closestNote.transform.position, transform.position));
                NoteCollection.ActiveNotes.Remove(closestNote);
                NoteCollection.NotePool.Pool.Release(closestNote);
            }
        }
        private void OnDestroy()
        {
            if (AssignedAction != null && InputHandler != null)
                AssignedAction.performed -= InputHandler;

            OnNoteHit = null;
        }
    }
}