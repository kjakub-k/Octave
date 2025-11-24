using KJakub.Octave.Data;
using KJakub.Octave.Game.Lines;
using KJakub.Octave.Game.Spawning;
using KJakub.Octave.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace KJakub.Octave.Game.Core
{
    public class GameController : MonoBehaviour, INoteCollection
    {
        [Header("Properties")]
        [SerializeField]
        private AccuracySetSO accuracySet;
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
        private GameObjectPool notePool;
        private List<GameObject> activeNotes = new();
        private PlayerInput inputSystem;
        public GameObjectPool NotePool { get { return notePool; } }
        public List<GameObject> ActiveNotes { get { return activeNotes; } }
        public GameStats GameStats { get { return stats; } }
        private List<AccuracySO> accuracies => accuracySet.Accuracies;
        private void Awake()
        {
            //so other scripts can connect to its events
            stats = new();
        }
        private void Start()
        {
            inputSystem = GetComponent<PlayerInput>();
            notePool = new(notePrefab, noteContainer, 40, 100);

            noteSpawner = new(this);
            noteDespawner = new(this);
        }
        private void Update()
        {
            noteDespawner.CheckIfOutOfBounds();
        }
        public void StartGame(SongData songData)
        {
            lineAmount = songData.Lines;
            stats.Reset();
            noteDespawner.OnNoteOutOfBounds += stats.Miss;
            lineManager.OnNoteHit += NoteHit;
            lineManager.GenerateLines(lineAmount, this, inputSystem);
            StartCoroutine(noteSpawner.SpawnNotes(lineManager.transform, lineAmount, songData.Notes, lineManager.LineLength));
        }
        private void NoteHit(float distance)
        {
            stats.AddCombo();
            AccuracySO nearestAccuracy = accuracies[accuracies.Count - 1];

            for (int i = 0; i < accuracies.Count; i++)
            {
                if (accuracies[accuracies.Count - 1 - i].Distance > distance)
                    nearestAccuracy = accuracies[i];
            }

            stats.AddToAccuracySet(nearestAccuracy);
        }
        public void EndGame()
        {
            noteSpawner.Stop();
            noteDespawner.DespawnAllNotes();
            noteDespawner.OnNoteOutOfBounds -= stats.Miss;
            lineManager.OnNoteHit -= NoteHit;
        }
    }
}