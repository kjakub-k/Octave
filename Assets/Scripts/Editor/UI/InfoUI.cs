using KJakub.Octave.Data;
using UnityEngine;
using UnityEngine.UIElements;
namespace KJakub.Octave.Editor.UI 
{ 
    public class InfoUI
    {
        private Label linesLabel;
        private Label bpmLabel;
        private Label snappingLabel;
        private Label songLengthLabel;
        public InfoUI(VisualElement root, SongData currentSongData)
        {
            linesLabel = root.Q<Label>("InfoLines");
            bpmLabel = root.Q<Label>("InfoBPM");
            snappingLabel = root.Q<Label>("InfoSnapping");
            songLengthLabel = root.Q<Label>("SongLength");

            currentSongData.OnLinesChanged += (int i) => UpdateLines(i);
            currentSongData.OnBPMChanged += (int i) => UpdateBPM(i);
            currentSongData.OnSnappingChanged += (SnappingType snap) => UpdateSnapping(snap);
            currentSongData.OnSongChanged += (AudioClip clip) => UpdateSongLength(clip.length);

            UpdateBPM(currentSongData.BPM);
            UpdateLines(currentSongData.Lines);
            UpdateSnapping(currentSongData.Snapping);
        }
        public void UpdateSongLength(float? lengthInSeconds)
        {
            string lengthText = lengthInSeconds?.ToString() ?? "---";

            songLengthLabel.text = $"Song Duration: {lengthText} sec.";
        }
        public void UpdateLines(int? lines)
        {
            string linesText = lines?.ToString() ?? "---";

            linesLabel.text = $"Lines: {linesText}";
        }
        public void UpdateBPM(int? bpm)
        {
            string bpmText = bpm?.ToString() ?? "---";

            bpmLabel.text = $"Beats Per Min.: {bpmText}";
        }
        public void UpdateSnapping(SnappingType? snapping)
        {
            int? snappingNum = (int?)snapping;
            string snap = "1/" + (snappingNum?.ToString() ?? "---");

            snappingLabel.text = $"Snapping: {snap}";
        }
    }
}