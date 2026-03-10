using KJakub.Octave.Data;
using KJakub.Octave.Game.Notes;
using System;
using UnityEngine;
namespace KJakub.Octave.Game.Core
{
    public class PracticeController : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;
        private SongData songData;
        private float start, end;
        private Coroutine coroutine;
        public event Action OnPracticeModeEntered;
        public SongData SongData { get { return songData; } }
        public float Timer { get { return gameController.NoteSpawner.PracticeTimer; } }
        public void StartPractice(SongData songData, LevelPlayerData lpd)
        {
            this.songData = songData;
            gameController.Practice(songData, lpd);
            start = 0;
            end = songData.Song.length;
            SpawnNotes(0);
            OnPracticeModeEntered?.Invoke();
        }
        public void Stop()
        {
            ChangeSpeed(1);
            gameController.NoteSpawner.Stop();
            StopCoroutine(coroutine);
        }
        public void SpawnNotes(float time)
        {
            gameController.NoteSpawner.Stop();

            if (coroutine != null)
                StopCoroutine(coroutine);

            gameController.NoteDespawner.DespawnAllNotes(gameController.NoteRuntimeCollection);
            coroutine = StartCoroutine(gameController.NoteSpawner.StartSpawningNotesFromTime(gameController.LineManager.transform,
                songData.Lines, songData.Notes, gameController.LineManager.LineLength / gameController.NotePrefab.GetComponent<NoteGO>().Speed, 
                start, end, time, gameController.NotePrefab.GetComponent<NoteGO>().Speed));
        }
        public void ChangeStartLoop(float newValue)
        {
            start = newValue;
            SpawnNotes(start);
        }
        public void ChangeEndLoop(float newValue)
        {
            end = newValue;
            SpawnNotes(start);
        }
        public void ChangeSpeed(float newValue)
        {
            Time.timeScale = newValue;
        }
    }
}