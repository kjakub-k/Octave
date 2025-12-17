using KJakub.Octave.Data;
using KJakub.Octave.Game.Camera;
using KJakub.Octave.Game.Lines;
using KJakub.Octave.Game.Spawning;
using KJakub.Octave.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
namespace KJakub.Octave.Game.Core
{
    public enum MusicStatus
    {
        Playing,
        NotPlaying
    }
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
        [Header("Properties - Environment")]
        [SerializeField]
        private Material defaultSharedMaterial;
        [SerializeField]
        private Color[] sharedMaterialColors;
        [SerializeField]
        private float colorChangeDuration;
        [Header("Properties - Thermometer")]
        [SerializeField]
        private float thermometerStartingValue;
        [SerializeField]
        private float thermometerDecreaseBy;
        [Header("Components")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private LineManager lineManager;
        [SerializeField]
        private CameraMover cameraMover;
        private GameStats stats;
        private NoteSpawner noteSpawner;
        private NoteDespawner noteDespawner;
        private Thermometer thermometer;
        private GameObjectPool notePool;
        private List<GameObject> activeNotes = new();
        private PlayerInput inputSystem;
        private MusicStatus musicStatus;
        public GameObjectPool NotePool { get { return notePool; } }
        public List<GameObject> ActiveNotes { get { return activeNotes; } }
        public GameStats GameStats { get { return stats; } }
        public Thermometer Thermometer { get { return thermometer; } }
        public MusicStatus MusicStatus { get { return musicStatus; } set { musicStatus = value; } }
        private List<AccuracySO> accuracies => accuracySet.Accuracies;
        private void Awake()
        {
            //so other scripts can connect to its events
            stats = new();
            thermometer = new(thermometerDecreaseBy, thermometerStartingValue);
        }
        private void Start()
        {
            inputSystem = GetComponent<PlayerInput>();
            notePool = new(notePrefab, noteContainer, 40, 100);

            noteSpawner = new(this);
            noteDespawner = new(this);

            thermometer.OnWeightChanged += ChangeDefaultColor;
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
            cameraMover.UpdateCamera(lineAmount * lineManager.LineWidth / 2);
            
            if (songData != null)
            {
                float delay = -(lineManager.LineLength / 10f);
                StartCoroutine(PlayMusic(delay, songData.Song));
            }

            StartCoroutine(noteSpawner.SpawnNotes(lineManager.transform, lineAmount, songData.Notes, lineManager.LineLength));
            StartCoroutine(thermometer.Decrease());
        }
        private void NoteMiss()
        {
            stats.Miss();
            stats.AddToScore(-10);
            thermometer.Add(-10);
        }
        private System.Collections.IEnumerator PlayMusic(float delay, AudioClip clip)
        {
            audioSource.clip = clip;

            yield return new WaitForSeconds(delay);

            while (musicStatus == MusicStatus.Playing)
            {
                if (audioSource.isPlaying == false)
                    audioSource.Play();

                yield return null;
            }
        }
        private void NoteHit(float distance)
        {
            stats.AddCombo();
            AccuracySO nearestAccuracy = accuracies[accuracies.Count - 1];

            foreach (var acc in accuracies)
            {
                if (distance <= acc.Distance)
                {
                    nearestAccuracy = acc;
                    break;
                }
            }

            thermometer.Add(nearestAccuracy.Weight * 10);
            stats.AddToScore(nearestAccuracy.Weight * 10 * thermometer.Weight);
            stats.AddToAccuracySet(nearestAccuracy);
        }
        private void ChangeDefaultColor(int weight)
        {
            if (weight == 10)
                defaultSharedMaterial.DOColor(sharedMaterialColors[3], colorChangeDuration);
            else if (weight == 5)
                defaultSharedMaterial.DOColor(sharedMaterialColors[2], colorChangeDuration);
            else if (weight == 2)
                defaultSharedMaterial.DOColor(sharedMaterialColors[1], colorChangeDuration);
            else if (weight == 1)
                defaultSharedMaterial.DOColor(sharedMaterialColors[0], colorChangeDuration);
        }
        public void EndGame()
        {
            noteSpawner.Stop();
            thermometer.Stop();
            noteDespawner.DespawnAllNotes();
            noteDespawner.OnNoteOutOfBounds -= stats.Miss;
            lineManager.OnNoteHit -= NoteHit;
            musicStatus = MusicStatus.NotPlaying;
        }
    }
}