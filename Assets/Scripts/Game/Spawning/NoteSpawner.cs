using KJakub.Octave.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private NoteRuntimeCollection noteCollection;
        private NoteSpawnerStatus status;
        private float practiceTimer;
        public NoteSpawnerStatus Status { get { return status; } }
        public float PracticeTimer { get { return practiceTimer; } set { practiceTimer = value; } }
        public NoteSpawner(NoteRuntimeCollection noteCollection)
        {
            this.noteCollection = noteCollection;
        }
        public void SpawnNote(Transform lineContainer, int lineIndex)
        {
            Transform line = lineContainer.GetChild(lineIndex);
            GameObject note = noteCollection.NotePool.Pool.Get();
            note.transform.position = line.position;
            noteCollection.ActiveNotes.Add(note);
        }
        public void SpawnNoteAt(Transform lineContainer, int lineIndex, float distance)
        {
            Transform line = lineContainer.GetChild(lineIndex);
            GameObject note = noteCollection.NotePool.Pool.Get();
            note.transform.position = line.position + Vector3.forward * distance;
            noteCollection.ActiveNotes.Add(note);
        }
        public void Stop()
        {
            status = NoteSpawnerStatus.NotSpawning;
        }
        public IEnumerator StartSpawningNotesFromTime(Transform lineContainer, int lineAmount, List<NoteData> notes, float travelTime, float start, float end, float current, float noteSpeed)
        {
            status = NoteSpawnerStatus.Spawning;
            practiceTimer = current;

            List<NoteData> shallowNotes = new(notes.Where(n => n.Time >= start && n.Time <= end).ToList());

            if (shallowNotes.Count <= 0)
            {
                Debug.Log("No notes");
                yield break;
            }

            List<NoteData> notesBeforeThat = new(notes.Where(n => n.Time < practiceTimer && n.Time >= practiceTimer - travelTime).ToList());
            List<NoteData> currentNotes = new(notes.Where(n => n.Time >= practiceTimer && n.Time <= end).ToList());

            for (int i = notesBeforeThat.Count - 1; i >= 0; i--)
            {
                SpawnNoteAt(lineContainer, lineAmount - notesBeforeThat[i].Lane - 1, noteSpeed * (practiceTimer - notesBeforeThat[i].Time));
                notesBeforeThat.RemoveAt(i);
            }

            while (true)
            {
                //using a for loop instead of a foreach because I am removing notes inside it;
                //foreach would break if i did so
                for (int i = currentNotes.Count - 1; i >= 0; i--)
                {
                    if (practiceTimer >= currentNotes[i].Time)
                    {
                        SpawnNote(lineContainer, lineAmount - currentNotes[i].Lane - 1); //reversed the lanes on accident
                        currentNotes.RemoveAt(i);
                    }
                }

                practiceTimer += Time.deltaTime;

                if (status == NoteSpawnerStatus.NotSpawning)
                    yield break;

                if (practiceTimer >= end)
                {
                    practiceTimer = start;
                    currentNotes = new(shallowNotes);
                }

                yield return null;
            }
        }
        public IEnumerator SpawnNotes(Transform lineContainer, int lineAmount, List<NoteData> notes, float laneLength, Action onFinishedSpawning, float onFinishedSpawningDelay)
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

                if (currentNotes.Count <= 0)
                {
                    status = NoteSpawnerStatus.NotSpawning;
                    yield return new WaitForSeconds(onFinishedSpawningDelay);
                    onFinishedSpawning?.Invoke();
                }

                yield return null;
            }
        }
    }
}