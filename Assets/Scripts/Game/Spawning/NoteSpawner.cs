using KJakub.Octave.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.Game.Spawning
{
    public class NoteSpawner
    {
        private INoteCollection noteCollection;
        public NoteSpawner(INoteCollection noteCollection)
        {
            this.noteCollection = noteCollection;
        }
        public void SpawnNote(Transform lineContainer, int lineIndex)
        {
            Transform line = lineContainer.GetChild(lineIndex);
            GameObject note = noteCollection.NotePool.Pool.Get();
            note.transform.position = line.position + Vector3.up * 0.5f;
            noteCollection.ActiveNotes.Add(note);
        }
        public IEnumerator SpawnRandomNotesCoroutine(Transform lineContainer, int lineAmount)
        {
            while (true)
            {
                SpawnNote(lineContainer, Random.Range(0, lineAmount));
                yield return new WaitForSeconds(1f);
            }
        }
        public IEnumerator SpawnNotes(List<NoteData> notes)
        {
            yield return null;
        }
    }
}