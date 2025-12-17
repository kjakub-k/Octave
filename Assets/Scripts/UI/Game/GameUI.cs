using KJakub.Octave.Game.Core;
using KJakub.Octave.ScriptableObjects;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
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
        private HealthBarUI healthBarUI;
        [SerializeField]
        private TMP_Text scoreLabel;
        [SerializeField]
        private TMP_Text comboLabel;
        [SerializeField]
        private Image deathScreen;
        private void Start()
        {
            gameController.GameStats.OnComboChanged += UpdateCombo;
            gameController.GameStats.OnHit += (AccuracySO acc) => latestAccuracyUI.ShowLabel(acc.Title + "!", acc.Color);
            gameController.GameStats.OnReset += ResetUI;
            gameController.GameStats.OnMiss += () => latestAccuracyUI.ShowLabel("Miss", Color.red);
            gameController.Thermometer.OnThermometerChanged += thermometerUI.UpdateValue;
            gameController.GameStats.OnScoreChanged += UpdateScore;
            gameController.Health.OnHealthRemoved += (int amount) => healthBarUI.UpdateHealth(amount, gameController.Health.MaxHealth);
            gameController.Health.OnHealthAdded += (int amount) => healthBarUI.UpdateHealth(amount, gameController.Health.MaxHealth);
            gameController.OnDefaultSharedColorChanged += (Color color) => healthBarUI.ChangeFillColor(color, gameController.ColorChangeDuration);
            gameController.OnLose += DeathScreen;
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
        private void DeathScreen()
        {
            DOTween.Sequence()
                .SetTarget(deathScreen)
                .Append(deathScreen.DOFade(0.5f, 2f))
                .Append(deathScreen.DOFade(0, 0)).SetUpdate(true);
        }
        private void ResetUI()
        {
            UpdateCombo(0, 0);
            UpdateScore(0);
        }
    }
}