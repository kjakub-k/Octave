using KJakub.Octave.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
namespace KJakub.Octave.Data
{
    public class GameStats
    {
        public List<AccuracyResult> HitsAccuracy { get; private set; }
        public int HighestCombo { get; private set; }
        public int Combo { get; private set; }
        public int Score { get; private set; }
        public int Misses { get; private set; }
        public float LevelLength { get; private set; }
        public int TotalHits { get
            {
                int totalHits = 0;

                foreach (var hit in HitsAccuracy)
                    totalHits++;

                return totalHits;
            } 
        }
        public event Action<int, int> OnComboChanged;
        public event Action OnMiss;
        public event Action OnReset;
        public event Action<AccuracyResult> OnHit;
        public event Action<int> OnScoreChanged;
        public GameStats() 
        {
            HitsAccuracy = new List<AccuracyResult>();
            Combo = 0;
            Misses = 0;
        }
        public float GetAccuracyPercentage()
        {
            int maxWeight = HitsAccuracy.Max(a => a.Accuracy.Weight);
            float earned = 0;
            float max = 0;

            foreach (var hit in HitsAccuracy)
            {
                earned += hit.Accuracy.Weight;
                max += maxWeight;
            }

            max += Misses * maxWeight;

            return max <= 0 ? 0 : (earned / max) * 100f;
        }
        public void SetLevelLength(float timeInSeconds)
        {
            LevelLength = timeInSeconds;
        }
        public void AddToAccuracySet(float timeInSecondsWhenHit, AccuracySO accuracy)
        {
            AccuracyResult accResult = new(timeInSecondsWhenHit, accuracy);
            HitsAccuracy.Add(accResult);

            OnHit?.Invoke(accResult);
        }
        public void Reset()
        {
            OnReset?.Invoke();
            HitsAccuracy.Clear();
            Combo = 0;
            HighestCombo = 0;
            Misses = 0;
            Score = 0;
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
        public void AddToScore(int score)
        {
            Score += score;
            OnScoreChanged?.Invoke(Score);
        }
    }
}