using KJakub.Octave.Data;
using KJakub.Octave.Game.Notes;
using UnityEngine;
namespace KJakub.Octave.Game.Core
{
    public class PracticeController : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;
        private SongData songData;
        private float start, end;
        public float Timer { get { return gameController.NoteSpawner.PracticeTimer; } }
        public void StartPractice(SongData songData)
        {
            this.songData = songData;
            gameController.Practice(songData);
            start = 0;
            end = songData.Song.length;
            SpawnNotes(0);
        }
        public void Stop()
        {
            ChangeSpeed(1);
        }
        public void SpawnNotes(float time)
        {
            gameController.NoteSpawner.Stop();
            gameController.NoteDespawner.DespawnAllNotes(gameController.NoteRuntimeCollection);
            StartCoroutine(gameController.NoteSpawner.StartSpawningNotesFromTime(gameController.LineManager.transform,
                songData.Lines, songData.Notes, gameController.LineManager.LineLength, 
                start, end, time));
        }
        public void ChangeStartLoop(float newValue)
        {
            start = newValue;
            SpawnNotes(start);
        }
        public void ChangeEndLoop(float newValue)
        {
            end = newValue;
            SpawnNotes(end);
        }
        public void ChangeSpeed(float newValue)
        {
            Time.timeScale = newValue;
        }
    }
}