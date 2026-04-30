using DG.Tweening;
using KJakub.Octave.Data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.LevelSelect
{
    public class ModificatorsUI : MonoBehaviour
    {
        [SerializeField]
        private Transform container;
        [SerializeField]
        private LevelSelectUI levelSelectUI;
        [Header("Components")]
        [SerializeField]
        private TMP_Text mirroredLabel;
        [SerializeField]
        private TMP_Text endlessLabel;
        [SerializeField]
        private TMP_Text noMissLabel;
        [SerializeField]
        private TMP_Text doubleSpeedLabel;
        [SerializeField]
        private TMP_Text halfSpeedLabel;
        public void Show()
        {
            container.gameObject.SetActive(true);
        }
        public void Hide()
        {
            container.gameObject.SetActive(false);
        }
        public void ToggleMirrored()
        {
            if (levelSelectUI.ActiveModifiers.Contains(GameModifier.Mirrored))
            {
                levelSelectUI.ActiveModifiers.Remove(GameModifier.Mirrored);
                mirroredLabel.text = "";
            }
            else
            {
                levelSelectUI.ActiveModifiers.Add(GameModifier.Mirrored);
                mirroredLabel.text = "X";
            }
        }
        public void ToggleEndless()
        {
            if (levelSelectUI.ActiveModifiers.Contains(GameModifier.Endless))
            {
                levelSelectUI.ActiveModifiers.Remove(GameModifier.Endless);
                endlessLabel.text = "";
            }
            else
            {
                levelSelectUI.ActiveModifiers.Add(GameModifier.Endless);
                endlessLabel.text = "X";
            }
        }
        public void ToggleNoMiss()
        {
            if (levelSelectUI.ActiveModifiers.Contains(GameModifier.NoMiss))
            {
                levelSelectUI.ActiveModifiers.Remove(GameModifier.NoMiss);
                noMissLabel.text = "";
            }
            else
            {
                levelSelectUI.ActiveModifiers.Add(GameModifier.NoMiss);
                noMissLabel.text = "X";
            }
        }
        public void ToggleDoubleSpeed()
        {
            if (levelSelectUI.ActiveModifiers.Contains(GameModifier.DoubleSpeed))
            {
                levelSelectUI.ActiveModifiers.Remove(GameModifier.DoubleSpeed);
                doubleSpeedLabel.text = "";
            }    
            else
            {
                levelSelectUI.ActiveModifiers.Add(GameModifier.DoubleSpeed);
                doubleSpeedLabel.text = "X";
            }

            if (levelSelectUI.ActiveModifiers.Contains(GameModifier.HalfSpeed))
            {
                levelSelectUI.ActiveModifiers.Remove(GameModifier.HalfSpeed);
                halfSpeedLabel.text = "";
            }
        }
        public void ToggleHalfSpeed()
        {
            if (levelSelectUI.ActiveModifiers.Contains(GameModifier.HalfSpeed))
            {
                levelSelectUI.ActiveModifiers.Remove(GameModifier.HalfSpeed);
                halfSpeedLabel.text = "";
            }
            else
            {
                levelSelectUI.ActiveModifiers.Add(GameModifier.HalfSpeed);
                halfSpeedLabel.text = "X";
            }

            if (levelSelectUI.ActiveModifiers.Contains(GameModifier.DoubleSpeed))
            {
                levelSelectUI.ActiveModifiers.Remove(GameModifier.DoubleSpeed);
                doubleSpeedLabel.text = "";
            }
        }
    }
}