using KJakub.Octave.Game.Spawning;
using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.Game.Lines
{
    public class LineCreator : MonoBehaviour
    {
        [SerializeField]
        private GameObject linePrefab;
        [SerializeField]
        private Transform noteDetectorContainer;
        [SerializeField]
        private GameObject noteDetector;
        public float LineWidth { get { return linePrefab.transform.localScale.x; } }
        public float LineHeight { get { return linePrefab.transform.localScale.y; } }
        public float LineLength { get { return linePrefab.transform.localScale.z; } }
        public void GenerateLines(int linesAmount, INoteCollection noteCollection)
        {
            List<Transform> children = new();

            foreach (Transform child in transform)
            {
                children.Add(child);
            }

            foreach (Transform child in children)
            {
                Destroy(child.gameObject);
            }

            float startX = -(linesAmount * LineWidth) / 2 + LineWidth / 2;

            for (int i = 0; i < linesAmount; i++)
            {
                Vector3 pos = new(transform.position.x + startX + i * LineWidth + LineWidth / 2, transform.position.y, transform.position.z);
                var line = Instantiate(linePrefab, pos, transform.rotation, transform);
                CreateNoteDetector(i, line, noteCollection);
            }
        }
        private void CreateNoteDetector(int key, GameObject line, INoteCollection noteCollection)
        {
            Vector3 pos = new(line.transform.position.x, line.transform.position.y + 0.5f, transform.position.z + LineLength - 0.5f);
            var btn = Instantiate(noteDetector, pos, Quaternion.identity, noteDetectorContainer);
            NoteDetector btnND = btn.GetComponent<NoteDetector>();
            btnND.NoteCollection = noteCollection;
            
        }
        private void OnDrawGizmos()
        {
            if (linePrefab == null)
                return;

            Gizmos.color = Color.purple;

            Vector3 prefabScale = linePrefab.GetComponentInChildren<MeshRenderer>().transform.localScale;
            Vector3 size = new(LineWidth * 7, LineHeight, LineLength);
            Vector3 centerPos = new(transform.position.x + LineWidth / 2, transform.position.y, transform.position.z + LineLength / 2);

            Gizmos.DrawCube(centerPos, size);
        }
    }
}