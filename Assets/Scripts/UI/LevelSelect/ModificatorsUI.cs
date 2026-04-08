using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.LevelSelect
{
    public class ModificatorsUI : MonoBehaviour
    {
        [SerializeField]
        private Transform container;
        public void Show()
        {
            container.gameObject.SetActive(true);
        }
        public void Hide()
        {
            container.gameObject.SetActive(false);
        }
    }
}