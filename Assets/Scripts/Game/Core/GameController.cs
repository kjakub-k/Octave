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
using KJakub.Octave.Game.Notes;
namespace KJakub.Octave.Game.Core
{
    public enum MusicStatus
    {
        Playing,
        NotPlaying
    }
    public class GameController : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField]
        private AccuracySetSO accuracySet;
        [SerializeField]
        private GameObject notePrefab;
        [SerializeField]
        private Transform noteContainer;
        [SerializeField]
        private int maxHealth;
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
        private NoteDespawner noteDespawner;
        [SerializeField]
        private CameraMover cameraMover;
        private GameStats stats;
        private NoteSpawner noteSpawner;
        private Thermometer thermometer;
        private Health health;
        private PressRecorder pressRecorder;
        private NoteRuntimeCollection noteRuntimeCollection;
        private PlayerInput inputSystem;
        private MusicStatus musicStatus;
        private SongData songData;
        private List<(float, int)?> presses = new List<(float, int)?>();
        public List<(float, int)?> Presses { get { return presses; } }
        public GameStats GameStats { get { return stats; } }
        public Thermometer Thermometer { get { return thermometer; } }
        public Health Health { get { return health; } }
        public MusicStatus MusicStatus { get { return musicStatus; } set { musicStatus = value; } }
        public float ColorChangeDuration { get { return colorChangeDuration; } }
        private List<AccuracySO> accuracies => accuracySet.Accuracies;
        public event Action<Color> OnDefaultSharedColorChanged;
        public event Action OnLose;
        public event Action OnFinished;
        private void Awake()
        {
            //so other scripts can connect to its events
            stats = new();
            pressRecorder = new();
            thermometer = new(thermometerDecreaseBy, thermometerStartingValue);
            health = new(maxHealth);
        }
        private void Start()
        {
            inputSystem = GetComponent<PlayerInput>();

            noteRuntimeCollection = new(notePrefab, noteContainer);
            noteSpawner = new(noteRuntimeCollection);

            thermometer.OnWeightChanged += ChangeDefaultColor;
            noteDespawner.OnNoteOutOfBounds += NoteMiss;
            lineManager.OnNoteHit += NoteHit;
        }
        public void PlayGame(SongData songData, List<(float, int)?> presses = null)
        {
            StartCoreGame(songData, presses);
            this.songData = songData;
            health.OnDeath += Death;
        }
        private void AddToPresses(float time, int line)
        {
            presses.Add((time, line));
        }
        public void StartCoreGame(SongData songData, List<(float, int)?> presses = null)
        {
            stats.Reset();
            lineManager.GenerateLines(songData.Lines, noteRuntimeCollection, inputSystem, (presses == null) ? true : false);

            if (presses == null)
            {
                this.presses.Clear();
                pressRecorder.OnPress += AddToPresses;
                StartCoroutine(pressRecorder.RecordPresses(inputSystem, songData.Lines));
            }
            else
            {
                StartCoroutine(pressRecorder.DoPresses(lineManager.NoteDetectors, presses));
            }

            cameraMover.UpdateCamera(songData.Lines * lineManager.LineWidth / 2);
            health.Heal(1000);

            float delay = lineManager.LineLength / notePrefab.GetComponent<NoteGO>().Speed;

            if (songData.Song != null)
                StartCoroutine(PlayMusic(delay, songData.Song));

            ChangeDefaultColor(1);
            StartCoroutine(noteSpawner.SpawnNotes(lineManager.transform, songData.Lines, songData.Notes, lineManager.LineLength, OnFinished, delay + 1f));
            StartCoroutine(noteDespawner.CheckIfOutOfBounds(noteRuntimeCollection));
            StartCoroutine(thermometer.Decrease());
        }
        private void NoteMiss()
        {
            stats.Miss();
            stats.AddToScore(-10);
            thermometer.Add(-10);
            health.Damage(20);
        }
        private System.Collections.IEnumerator PlayMusic(float delay, AudioClip clip)
        {
            audioSource.clip = clip;
            musicStatus = MusicStatus.Playing;

            yield return new WaitForSeconds(delay);

            while (musicStatus == MusicStatus.Playing)
            {
                if (audioSource.isPlaying == false)
                    audioSource.Play();

                yield return null;
            }

            audioSource.Stop();
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

            thermometer.Add(nearestAccuracy.Weight * 2);
            health.Heal(nearestAccuracy.Weight * 5);
            stats.AddToScore(nearestAccuracy.Weight * 10 * thermometer.Weight);
            stats.AddToAccuracySet(nearestAccuracy);
        }
        private void ChangeDefaultColor(int weight)
        {
            int index = -1;

            if (weight == 10)
                index = 3;
            else if (weight == 5)
                index = 2;
            else if (weight == 2)
                index = 1;
            else if (weight == 1)
                index = 0;

            if (index != -1)
            {
                defaultSharedMaterial.DOColor(sharedMaterialColors[index], colorChangeDuration);
                OnDefaultSharedColorChanged?.Invoke(sharedMaterialColors[index]);
            }
        }
        private void Death()
        {
            health.OnDeath -= Death;
            StartCoroutine(DeathScreen());
        }
        private System.Collections.IEnumerator DeathScreen()
        {
            OnLose?.Invoke();

            DOTween.To(
                () => Time.timeScale,
                x =>
                {
                    Time.timeScale = x;
                },
                0f,
                1f
            ).SetUpdate(true);

            yield return new WaitForSecondsRealtime(2f);

            EndCoreGame();
            Time.timeScale = 1f;
            OnFinished?.Invoke();
        }
        public void EndGame()
        {
            StopCoroutine(DeathScreen());
            Time.timeScale = 1f;
            EndCoreGame();
            health.OnDeath -= Death;
        }
        public void EndCoreGame()
        {
            noteSpawner.Stop();
            pressRecorder.Stop();
            pressRecorder.OnPress -= AddToPresses;
            thermometer.Stop();
            noteDespawner.Stop();
            noteDespawner.DespawnAllNotes(noteRuntimeCollection);
            musicStatus = MusicStatus.NotPlaying;
        }
    }
}