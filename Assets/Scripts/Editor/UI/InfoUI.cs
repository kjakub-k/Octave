using KJakub.Octave.Data;
using UnityEngine.UIElements;
namespace KJakub.Octave.Editor.UI 
{ 
    public class InfoUI
    {
        private Label linesLabel;
        private Label bpmLabel;
        private Label snappingLabel;
        public InfoUI(VisualElement root, SongData currentSongData)
        {
            linesLabel = root.Q<Label>("InfoLines");
            bpmLabel = root.Q<Label>("InfoBPM");
            snappingLabel = root.Q<Label>("InfoSnapping");

            currentSongData.OnLinesChanged += (int i) => UpdateLines(i);
            currentSongData.OnBPMChanged += (int i) => UpdateBPM(i);
            currentSongData.OnSnappingChanged += (SnappingType snap) => UpdateSnapping(snap);

            UpdateBPM(currentSongData.BPM);
            UpdateLines(currentSongData.Lines);
            UpdateSnapping(currentSongData.Snapping);
        }
        public void UpdateLines(int? lines)
        {
            string linesText;

            if (lines == null)
            {
                linesText = "---";
            } else
            {
                linesText = $"{lines}";
            }

            linesLabel.text = $"Lines: {lines}";
        }
        public void UpdateBPM(int? bpm)
        {
            string bpmText;

            if (bpm == null)
            {
                bpmText = "---";
            }
            else
            {
                bpmText = $"{bpm}";
            }

            bpmLabel.text = $"Beats Per Min.: {bpmText}";
        }
        public void UpdateSnapping(SnappingType? snapping)
        {
            string snap;

            switch (snapping)
            {
                case SnappingType.Full:
                    snap = "1/1";
                    break;
                case SnappingType.Half:
                    snap = "1/2";
                    break;
                case SnappingType.Quarter:
                    snap = "1/4";
                    break;
                case SnappingType.Eighth:
                    snap = "1/8";
                    break;
                case SnappingType.Sixteenth:
                    snap = "1/16";
                    break;
                default:
                    snap = "---";
                    break;
            }

            snappingLabel.text = $"Snapping: {snap}";
        }
    }
}