using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.Game.Spawning
{
    public class NoteRuntimeCollection
    {
        public GameObjectPool NotePool { get; }
        public List<GameObject> ActiveNotes { get; }
        public NoteRuntimeCollection(GameObject prefab, Transform container)
        {
            NotePool = new(prefab, container, 40, 100);
            ActiveNotes = new List<GameObject>();
        }
    }
}