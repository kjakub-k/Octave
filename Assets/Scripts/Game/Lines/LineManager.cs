using KJakub.Octave.Game.Spawning;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace KJakub.Octave.Game.Lines
{
    public class LineManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject linePrefab;
        [SerializeField]
        private GameObject lineSidePrefab;
        [SerializeField]
        private Transform lineSideContainer;
        [SerializeField]
        private Transform noteDetectorContainer;
        [SerializeField]
        private GameObject noteDetector;
        [SerializeField]
        private float detectionRadius;
        private NoteDetector[] noteDetectors;
        public NoteDetector[] NoteDetectors { get { return noteDetectors; } }
        public event Action<float> OnNoteHit;
        public float LineWidth { get { return linePrefab.GetComponentInChildren<Renderer>().bounds.size.x; } }
        public float LineHeight { get { return linePrefab.GetComponentInChildren<Renderer>().bounds.size.y; } }
        public float LineLength { get { return linePrefab.GetComponentInChildren<Renderer>().bounds.size.z; } }
        public void GenerateLines(int linesAmount, NoteRuntimeCollection noteCollection, PlayerInput inputSystem, bool pressable)
        {
            List<Transform> children = new();
            this.noteDetectors = new NoteDetector[linesAmount];

            foreach (Transform child in transform)
            {
                children.Add(child);
            }

            foreach (Transform child in children)
            {
                Destroy(child.gameObject);
            }

            List<Transform> noteDetectors = new();

            foreach (Transform child in noteDetectorContainer)
            {
                noteDetectors.Add(child);
            }

            foreach (Transform noteDetector in noteDetectors)
            {
                
                Destroy(noteDetector.gameObject);
            }

            for (int i = 0; i < linesAmount; i++)
            {
                Vector3 pos = new(transform.position.x + i * LineWidth + LineWidth / 2, transform.position.y, transform.position.z);
                var line = Instantiate(linePrefab, pos, transform.rotation, transform);
                var btnND = CreateNoteDetector(i, line, noteCollection, (pressable == true) ? inputSystem.actions.FindAction($"Line_{linesAmount - i - 1}") : null); //because i accidentally reversed the lines
                this.noteDetectors[i] = btnND;
            }

            CreateLineSides(linesAmount);
        }
        private void CreateLineSides(int linesAmount)
        {
            List<Transform> lineSides = new();

            foreach (Transform lineSide in lineSideContainer)
            {
                lineSides.Add(lineSide);
            }

            foreach (Transform lineSide in lineSides)
            {
                Destroy(lineSide.gameObject);
            }

            Renderer renderer = lineSidePrefab.GetComponentInChildren<Renderer>();

            Instantiate(lineSidePrefab, lineSideContainer.transform.position + Vector3.left * renderer.bounds.extents.x, Quaternion.identity, lineSideContainer);
            Instantiate(lineSidePrefab, lineSideContainer.transform.position + Vector3.right * LineWidth * linesAmount + Vector3.right * renderer.bounds.extents.x, Quaternion.identity, lineSideContainer);
        }
        private NoteDetector CreateNoteDetector(int lineNumber, GameObject line, NoteRuntimeCollection noteCollection, InputAction inputAction)
        {
            Renderer renderer = noteDetector.GetComponentInChildren<Renderer>();
            Vector3 pos = new(line.transform.position.x, line.transform.position.y + LineHeight, transform.position.z + LineLength - renderer.bounds.size.z / 2);
            var btn = Instantiate(noteDetector, pos, Quaternion.identity, noteDetectorContainer);
            NoteDetector btnND = btn.GetComponent<NoteDetector>();
            btnND.NoteCollection = noteCollection;
            btnND.DetectionSize = detectionRadius;

            void OnPerformed(InputAction.CallbackContext ctx) => btnND.OnNoteDetectorPress();
            void OnCancelled(InputAction.CallbackContext ctx) => btnND.ChangeMaterial(0);

            btnND.PerformedInputHandler = OnPerformed;
            btnND.CanceledInputHandler = OnCancelled;
            btnND.OnNoteHit += OnNoteHit;

            if (inputAction != null)
            {
                inputAction.performed += OnPerformed;
                inputAction.canceled += OnCancelled;
                btnND.AssignedAction = inputAction;
            }

            return btnND;
        }
        private void OnDrawGizmos()
        {
            if (linePrefab == null)
                return;

            Gizmos.color = Color.purple;

            Vector3 prefabScale = linePrefab.GetComponentInChildren<MeshRenderer>().transform.localScale;
            Vector3 size = new(LineWidth * 7, LineHeight, LineLength);
            Vector3 centerPos = new(transform.position.x + LineWidth * 3.5f, transform.position.y, transform.position.z + LineLength / 2);

            Gizmos.DrawCube(centerPos, size);
        }
    }
}