using KJakub.Octave.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AccuracySet", menuName = "Octave/Accuracy Set")]
    public class AccuracySetSO : ScriptableObject
    {
        public List<AccuracySO> accuracies;
    }
}