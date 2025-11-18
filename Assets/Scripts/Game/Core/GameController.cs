using KJakub.Octave.Data;
using KJakub.Octave.Game.Interfaces;
using KJakub.Octave.Game.Lines;
using KJakub.Octave.Game.Spawning;
using System;
using System.Collections.Generic;
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
        private LineCreator lineManager;
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
        public void StartGame()
        {
            lineManager.GenerateLines(lineAmount, this, inputSystem);
            StartCoroutine(noteSpawner.SpawnRandomNotesCoroutine(lineManager.transform, lineAmount));
        }
        public void StartGame(SongData songData)
        {
            AudioClip clip = songData.Song;
            lineAmount = songData.Lines;
            lineManager.GenerateLines(lineAmount, this, inputSystem);
            StartCoroutine(noteSpawner.SpawnRandomNotesCoroutine(lineManager.transform, lineAmount));
        }
        private void Update()
        {
            noteDespawner.CheckIfOutOfBounds();
        }
        public void EndGame()
        {
            throw new NotImplementedException();
        }
    }
}