using UnityEngine;
using UnityEngine.UIElements;
namespace KJakub.Octave.UI.Game
{
    public class LatestAccuracyUI
    {
        private Label accuracyLabel;
        public LatestAccuracyUI(Label accuracyLabel)
        {
            this.accuracyLabel = accuracyLabel;
        }
        public void ShowLabel(string title, Color color)
        {
            accuracyLabel.text = title;
            accuracyLabel.style.color = new StyleColor(color);
            accuracyLabel.RemoveFromClassList("show");
            accuracyLabel.RemoveFromClassList("hidden");

            accuracyLabel.schedule.Execute(() =>
            {
                accuracyLabel.AddToClassList("hidden");

                accuracyLabel.schedule.Execute(() =>
                {
                    accuracyLabel.RemoveFromClassList("hidden");
                    accuracyLabel.AddToClassList("show");

                }).StartingIn(1);

            }).StartingIn(1);

            accuracyLabel.schedule.Execute(() =>
            {
                accuracyLabel.RemoveFromClassList("show");
                accuracyLabel.AddToClassList("hidden");
            }).StartingIn(700);
        }
    }
}