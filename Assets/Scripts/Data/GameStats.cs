using KJakub.Octave.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
namespace KJakub.Octave.Data
{
    public class GameStats
    {
        public List<AccuracySO> HitsAccuracy { get; private set; }
        public int HighestCombo { get; private set; }
        public int Combo { get; private set; }
        public int Score { get { return CalculateScore(); } }
        public int Misses { get; private set; }
        public event Action<int, int> OnComboChanged;
        public event Action OnMiss;
        public event Action OnReset;
        public event Action<AccuracySO> OnHit;
        public GameStats() 
        {
            HitsAccuracy = new List<AccuracySO>();
            Combo = 0;
            Misses = 0;
        }
        public void AddToAccuracySet(AccuracySO acc)
        {
            HitsAccuracy.Add(acc);

            OnHit?.Invoke(acc);
        }
        public void Reset()
        {
            OnReset?.Invoke();
            HitsAccuracy.Clear();
            Combo = 0;
            HighestCombo = 0;
            Misses = 0;
        }
        public void Miss()
        {
            SetCombo(0);
            Misses++;

            OnMiss?.Invoke();
        }
        public void AddCombo()
        {
            Combo++;

            if (Combo > HighestCombo)
                HighestCombo = Combo;

            OnComboChanged?.Invoke(Combo, HighestCombo);
        }
        public void SetCombo(int num)
        {
            Combo = num;

            if (Combo > HighestCombo)
                HighestCombo = Combo;

            OnComboChanged?.Invoke(Combo, HighestCombo);
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