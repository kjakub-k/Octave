using KJakub.Octave.Data;
using KJakub.Octave.Game.Interfaces;
using KJakub.Octave.Game.Lines;
using KJakub.Octave.Game.Spawning;
using System;
using System.Collections.Generic;
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
        private GameObjectPool notePool;
        private List<GameObject> activeNotes = new();
        private PlayerInput inputSystem;
        public GameObjectPool NotePool { get { return notePool; } }
        public List<GameObject> ActiveNotes { get { return activeNotes; } }
        private void Start()
        {
            inputSystem = GetComponent<PlayerInput>();
            notePool = new(notePrefab, noteContainer, 40, 100);

            noteSpawner = new(this);
            noteDespawner = new(this);
        }
        public void StartGame(SongData songData)
        {
            lineAmount = songData.Lines;
            noteDespawner.OnNoteOutOfBounds += () =>
            {
                stats.SetCombo(0);
                stats.Misses++;
                Debug.Log($"Misses: {stats.Misses}");
            };
            lineManager.OnNoteHit += (float distance) =>
            {
                stats.AddCombo();
                Debug.Log($"Combo: {stats.Combo}:{stats.HighestCombo}");
            };
            lineManager.GenerateLines(lineAmount, this, inputSystem);
            stats = new();
            StartCoroutine(noteSpawner.SpawnNotes(lineManager.transform, lineAmount, songData.Notes, lineManager.LineLength));
        }
        private void Update()
        {
            noteDespawner.CheckIfOutOfBounds();
        }
        public void EndGame()
        {
            noteSpawner.Stop();
            noteDespawner.DespawnAllNotes();
        }
    }
}