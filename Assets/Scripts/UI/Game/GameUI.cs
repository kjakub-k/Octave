using KJakub.Octave.Game.Core;
using KJakub.Octave.ScriptableObjects;
using UnityEngine;
using UnityEngine.UIElements;
namespace KJakub.Octave.UI.Game
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private VisualTreeAsset gameLayout;
        [Header("Managers")]
        [SerializeField]
        private GameController gameController;
        private LatestAccuracyUI latestAccuracyUI;
        private Label scoreLabel;
        private Label comboLabel;
        private Label highestComboLabel;
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            InitializeElements(root);

            gameController.GameStats.OnComboChanged += UpdateCombo;
            gameController.GameStats.OnHit += (AccuracySO acc) =>
            {
                latestAccuracyUI.ShowLabel(acc.Title + "!");
                UpdateScore(gameController.GameStats.Score);
            };
            gameController.GameStats.OnReset += ResetUI;
            gameController.GameStats.OnMiss += () =>
            {
                latestAccuracyUI.ShowLabel("Miss");
                UpdateScore(gameController.GameStats.Score);
            };
        }
        private void InitializeElements(VisualElement root)
        {
            scoreLabel = root.Q<Label>("Score");
            comboLabel = root.Q<Label>("Combo");
            highestComboLabel = root.Q<Label>("HighestCombo");
            latestAccuracyUI = new(root.Q<Label>("Accuracy"));
        }
        private void UpdateScore(int score)
        {
            scoreLabel.text = score.ToString();
        }
        private void UpdateCombo(int combo, int highestCombo)
        {
            comboLabel.text = combo.ToString();
            highestComboLabel.text = highestCombo.ToString();
        }
        private void ResetUI()
        {
            UpdateCombo(0, 0);
            UpdateScore(0);
        }
    }
}