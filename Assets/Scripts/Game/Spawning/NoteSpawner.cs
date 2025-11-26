using KJakub.Octave.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.Game.Spawning
{
    public enum NoteSpawnerStatus
    {
        Spawning = 1,
        NotSpawning = 0
    }
    public class NoteSpawner
    {
        private INoteCollection noteCollection;
        private NoteSpawnerStatus status;
        public NoteSpawnerStatus Status { get { return status; } }
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
        public void Stop()
        {
            status = NoteSpawnerStatus.NotSpawning;
        }
        public IEnumerator SpawnRandomNotesCoroutine(Transform lineContainer, int lineAmount)
        {
            status = NoteSpawnerStatus.Spawning;

            while (status == NoteSpawnerStatus.Spawning)
            {
                SpawnNote(lineContainer, Random.Range(0, lineAmount));
                yield return new WaitForSeconds(1f);
            }
        }
        public IEnumerator SpawnNotes(Transform lineContainer, int lineAmount, List<NoteData> notes, float laneLength)
        {
            status = NoteSpawnerStatus.Spawning;
            float timer = 0;
            //creating a shallow list because we are not modifying notes
            List<NoteData> currentNotes = new(notes);

            while (status == NoteSpawnerStatus.Spawning)
            {
                //using a for loop instead of a foreach because I am removing notes inside it;
                //foreach would break if i did so
                for (int i = currentNotes.Count - 1; i >= 0; i--)
                {
                    if (timer >= currentNotes[i].Time)
                    {
                        SpawnNote(lineContainer, lineAmount - currentNotes[i].Lane - 1); //reversed the lanes on accident
                        currentNotes.RemoveAt(i);
                    }
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}