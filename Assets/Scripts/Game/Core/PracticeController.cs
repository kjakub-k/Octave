using KJakub.Octave.Data;
using UnityEngine;
namespace KJakub.Octave.Game.Core
{
    public class PracticeController : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;
        public void StartPractice(SongData songData)
        {
            gameController.Practice(songData);
        }
        public void Stop()
        {

        }
        public void ChangeTime(float time)
        {

        }
        public void ChangeStartLoop(float newValue)
        {

        }
        public void ChangeEndLoop(float newValue)
        {

        }
        public void ChangeSpeed(float newValue)
        {

        }
    }
}