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
            gameController.GameStats.OnHit += SpawnAccuracy;
            gameController.GameStats.OnReset += ResetUI;
            gameController.GameStats.OnMiss += () => UpdateScore(gameController.GameStats.Score);
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
        private void SpawnAccuracy(AccuracySO accuracy)
        {
            VisualElement accElement = new();
            accElement.AddToClassList("accuracy");
            accElement.AddToClassList($"{accuracy.Title.ToLower()}");

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