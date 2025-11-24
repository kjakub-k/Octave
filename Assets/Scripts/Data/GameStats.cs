using System.Collections.Generic;
using KJakub.Octave.ScriptableObjects;
namespace KJakub.Octave.Data
{
    public class GameStats
    {
        public List<AccuracySO> HitsAccuracy { get; set; }
        public int HighestCombo { get; private set; }
        public int Combo { get; private set; }
        public int Score { get { return CalculateScore(); } }
        public int Misses { get; set; }
        public GameStats() 
        {
            HitsAccuracy = new List<AccuracySO>();
            Combo = 0;
            Misses = 0;
        }
        public void Reset()
        {
            HitsAccuracy.Clear();
            Combo = 0;
            HighestCombo = 0;
            Misses = 0;
        }
        public void AddCombo()
        {
            Combo++;

            if (Combo > HighestCombo)
                HighestCombo = Combo;
        }
        public void SetCombo(int num)
        {
            Combo = num;

            if (Combo > HighestCombo)
                HighestCombo = Combo;
        }
        private int CalculateScore()
        {
            int score = 0;

            foreach (var accuracy in HitsAccuracy)
            {
                score += accuracy.Weight * 10;
            }

            score -= Misses * 10;
            
            return score;
        }
    }
}