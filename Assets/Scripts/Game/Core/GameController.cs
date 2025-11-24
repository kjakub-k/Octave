using KJakub.Octave.Data;
using KJakub.Octave.Game.Interfaces;
using KJakub.Octave.Game.Lines;
using KJakub.Octave.Game.Spawning;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;
namespace KJakub.Octave.Game.Core
{
    public class GameController : MonoBehaviour, INoteCollection, IGameController
    {
        [Header("Properties")]
        [SerializeField]
        [Range(0, 7)]
        private int lineAmount;
        [SerializeField]
        private GameObject notePrefab;
        [SerializeField]
        private Transform noteContainer;
        [Header("Managers")]
        [SerializeField]
        private LineManager lineManager;
        private GameStats stats;
        private NoteSpawner noteSpawner;
        private NoteDespawner noteDespawner;
        private List<Accuracy> accuracies = new(); //TODO: do this with scriptable objects instead
        private GameObjectPool notePool;
        private List<GameObject> activeNotes = new();
        private PlayerInput inputSystem;
        public GameObjectPool NotePool { get { return notePool; } }
        public List<GameObject> ActiveNotes { get { return activeNotes; } }
        private void Start()
        {
            AddAccuracies();
            inputSystem = GetComponent<PlayerInput>();
            notePool = new(notePrefab, noteContainer, 40, 100);

            noteSpawner = new(this);
            noteDespawner = new(this);
            stats = new();
        }
        [Obsolete("Should be replaced with scriptable objects later")]
        private void AddAccuracies()
        {
            accuracies.Add(new("Perfect", 0.1f, 4));
            accuracies.Add(new("Great", 0.2f, 3));
            accuracies.Add(new("Ok", 0.5f, 2));
            accuracies.Add(new("Bad", 1f, 1));
        }
        private void Update()
        {
            noteDespawner.CheckIfOutOfBounds();
        }
        public void StartGame(SongData songData)
        {
            lineAmount = songData.Lines;
            stats.Reset();
            noteDespawner.OnNoteOutOfBounds += NoteMiss;
            lineManager.OnNoteHit += NoteHit;
            lineManager.GenerateLines(lineAmount, this, inputSystem);
            StartCoroutine(noteSpawner.SpawnNotes(lineManager.transform, lineAmount, songData.Notes, lineManager.LineLength));
        }
        private void NoteMiss()
        {
            stats.SetCombo(0);
            stats.Misses++;
            Debug.Log($"Misses: {stats.Misses}");
        }
        private void NoteHit(float distance)
        {
            stats.AddCombo();
            Accuracy nearestAccuracy = accuracies[3];

            for (int i = 0; i < accuracies.Count; i++)
            {
                if (accuracies[3 - i].Distance > distance)
                    nearestAccuracy = accuracies[i];
            }

            Debug.Log($"Acc: {nearestAccuracy.Title}");
            stats.HitsAccuracy.Add(nearestAccuracy);

            Debug.Log($"Combo: {stats.Combo}:{stats.HighestCombo}");
        }
        public void EndGame()
        {
            noteSpawner.Stop();
            noteDespawner.DespawnAllNotes();
            noteDespawner.OnNoteOutOfBounds -= NoteMiss;
            lineManager.OnNoteHit -= NoteHit;
        }
    }
}