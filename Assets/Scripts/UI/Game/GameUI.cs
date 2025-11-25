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
        private Label scoreLabel;
        private Label comboLabel;
        private Label highestComboLabel;
        private VisualElement accuracyContainer;
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            InitializeElements(root);

            gameController.GameStats.OnComboChanged += UpdateCombo;
            gameController.GameStats.OnHit += (AccuracySO acc) => SpawnAccuracy(acc.Title, acc.Weight * 10);
            gameController.GameStats.OnReset += ResetUI;
            gameController.GameStats.OnMiss += () => UpdateScore(gameController.GameStats.Score);
            gameController.GameStats.OnMiss += () => SpawnAccuracy("Miss", -10);
        }
        private void InitializeElements(VisualElement root)
        {
            scoreLabel = root.Q<Label>("Score");
            comboLabel = root.Q<Label>("Combo");
            highestComboLabel = root.Q<Label>("HighestCombo");
            accuracyContainer = root.Q<VisualElement>("AccuracyContainer");
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
        private void SpawnAccuracy(string title, int score)
        {
            VisualElement accElement = new();
            Label label = new();
            accElement.AddToClassList("accuracy");
            accElement.AddToClassList($"{title.ToLower()}");
            label.text = $"{title} ({((score > 0) ? '+' + score.ToString() : score.ToString())})";
            accElement.Add(label);

            accuracyContainer.Add(accElement);

            UpdateScore(gameController.GameStats.Score);
        }
        private void ResetUI()
        {
            UpdateCombo(0, 0);
            UpdateScore(0);
            accuracyContainer.Clear();
        }
    }
}