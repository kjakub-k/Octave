using KJakub.Octave.Game.Core;
using KJakub.Octave.ScriptableObjects;
using UnityEngine;
using TMPro;
namespace KJakub.Octave.UI.Game
{
    public class GameUI : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField]
        private GameController gameController;
        [Header("Components")]
        [SerializeField]
        private LatestAccuracyUI latestAccuracyUI;
        [SerializeField]
        private HighestComboUI highestComboUI;
        [SerializeField]
        private ThermometerUI thermometerUI;
        [SerializeField]
        private TMP_Text scoreLabel;
        [SerializeField]
        private TMP_Text comboLabel;
        private void Start()
        {
            gameController.GameStats.OnComboChanged += UpdateCombo;
            gameController.GameStats.OnHit += (AccuracySO acc) => latestAccuracyUI.ShowLabel(acc.Title + "!", acc.Color);
            gameController.GameStats.OnReset += ResetUI;
            gameController.GameStats.OnMiss += () => latestAccuracyUI.ShowLabel("Miss", Color.red);
            gameController.Thermometer.OnThermometerChanged += thermometerUI.UpdateValue;
            gameController.GameStats.OnScoreChanged += UpdateScore;
        }
        private void UpdateScore(int score)
        {
            scoreLabel.text = score.ToString();
        }
        private void UpdateCombo(int combo, int highestCombo)
        {
            comboLabel.text = combo.ToString();
            highestComboUI.ChangeText(highestCombo.ToString());

            if (combo < highestCombo)
            {
                highestComboUI.Show();
            }

            if (combo >= highestCombo)
            {
                highestComboUI.Hide();
            }
        }
        private void ResetUI()
        {
            UpdateCombo(0, 0);
            UpdateScore(0);
        }
    }
}