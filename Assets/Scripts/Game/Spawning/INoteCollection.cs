using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.Game.Spawning
{
    public interface INoteCollection
    {
        public GameObjectPool NotePool { get; }
        public List<GameObject> ActiveNotes { get; }
    }
}