using KJakub.Octave.ScriptableObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace KJakub.Octave.Data
{
    [Serializable]
    public class GameStats
    {
        [JsonIgnore]
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
            if (HitsAccuracy.Count <= 0)
                return 0;

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
        public void AddResultsToPlayerPrefs()
        {
            PlayerPrefs.SetInt("TotalScore", 
                PlayerPrefs.GetInt("TotalScore", 0) + Score);
            PlayerPrefs.SetInt("TotalMisses",
                PlayerPrefs.GetInt("TotalMisses", 0) + Misses);
            PlayerPrefs.SetInt("TotalHits",
                PlayerPrefs.GetInt("TotalHits", 0) + TotalHits);
            PlayerPrefs.SetInt("GamesPlayed",
                PlayerPrefs.GetInt("GamesPlayed", 0) + 1);
            PlayerPrefs.SetFloat("AverageAccuracy",
                (PlayerPrefs.GetFloat("AverageAccuracy", 100) + GetAccuracyPercentage()) / 2);

            if (PlayerPrefs.GetInt("HighestCombo", 0) < HighestCombo)
                PlayerPrefs.SetInt("HighestCombo", HighestCombo);
        }
    }
}