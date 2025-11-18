using KJakub.Octave.Data;
namespace KJakub.Octave.Game.Interfaces
{
    public interface IGameController
    {
        public void StartGame();
        public void StartGame(SongData songData);
        public void EndGame();
    }
}