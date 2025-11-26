using UnityEngine;
namespace KJakub.Octave.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Accuracy", menuName = "Octave/Accuracy")]
    public class AccuracySO : ScriptableObject
    {
        public string Title;
        public float Distance;
        public int Weight;
        public Color Color;
    }
}