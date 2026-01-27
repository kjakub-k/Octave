using KJakub.Octave.ScriptableObjects;
using UnityEngine;
namespace KJakub.Octave.Data
{
    public class AccuracyResult
    {
        public float TimeHitInSeconds { get; private set; }
        public AccuracySO Accuracy { get; private set; }
        public int Weight { get { return Accuracy.Weight; } }
        public Color Color { get { return Accuracy.Color; } }
        public AccuracyResult(float timeHitInSeconds, AccuracySO accuracy)
        {
            (TimeHitInSeconds, Accuracy) = (timeHitInSeconds, accuracy);
        }
    }
}