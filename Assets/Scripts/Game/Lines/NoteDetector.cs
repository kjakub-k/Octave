using KJakub.Octave.Game.Spawning;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
namespace KJakub.Octave.Game.Lines
{
    public class NoteDetector : MonoBehaviour
    {
        [SerializeField]
        private Material[] materials;
        [SerializeField]
        private Renderer render;
        public float DetectionSize { get; set; }
        public NoteRuntimeCollection NoteCollection { get; set; }
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
            ChangeMaterial(1);
            Debug.Log(closestNote);

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

            foreach (var note in NoteCollection.ActiveNotes)
            {
                if (note.transform.position.x != transform.position.x)
                    continue;

                float distance = transform.position.z - note.transform.position.z;

                if (distance <= DetectionSize)
                {
                    if (closestNote == null)
                        closestNote = note;

                    if (closestNote.transform.position.z < note.transform.position.z)
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