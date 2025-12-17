using KJakub.Octave.Game.Spawning;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace KJakub.Octave.Game.Lines
{
    public class NoteDetector : MonoBehaviour
    {
        [SerializeField]
        private Material[] materials;
        [SerializeField]
        private Renderer render;
        public float DetectionRadius { get; set; }
        public INoteCollection NoteCollection { get; set; }
        public int Lane { get; set; }
        public InputAction AssignedAction { get; set; }
        public Action<InputAction.CallbackContext> PerformedInputHandler { get; set; }
        public Action<InputAction.CallbackContext> CanceledInputHandler { get; set; }
        public event Action<float> OnNoteHit;
        public void ChangeMaterial(int index)
        {
            render.material = materials[index];
        }
        public void OnNoteDetectorPress()
        {
            GameObject closestNote = GetClosestNote();

            if (closestNote != null)
            {
                float distance = Mathf.Abs(transform.position.z - closestNote.transform.position.z);
                OnNoteHit?.Invoke(distance);
                NoteCollection.ActiveNotes.Remove(closestNote);
                NoteCollection.NotePool.Pool.Release(closestNote);
            }
        }
        private GameObject GetClosestNote()
        {
            GameObject closestNote = null;
            float closestDistance = float.PositiveInfinity;

            foreach (var note in NoteCollection.ActiveNotes)
            {
                if (note.transform.position.x != transform.position.x)
                    continue;

                float deltaZ = transform.position.z - note.transform.position.z;

                if (deltaZ < 0 || deltaZ > DetectionRadius)
                    continue;

                if (deltaZ < closestDistance)
                {
                    closestDistance = deltaZ;
                    closestNote = note;
                }
            }

            return closestNote;
        }
        private void OnDestroy()
        {
            if (AssignedAction != null && PerformedInputHandler != null)
                AssignedAction.performed -= PerformedInputHandler;

            if (AssignedAction != null && CanceledInputHandler != null)
                AssignedAction.canceled -= CanceledInputHandler;

            OnNoteHit = null;
        }
    }
}