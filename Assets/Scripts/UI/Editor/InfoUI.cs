using KJakub.Octave.Data;
using UnityEngine;
using UnityEngine.UIElements;
namespace KJakub.Octave.UI.Editor
{ 
    public class InfoUI
    {
        private Label linesLabel;
        private Label bpmLabel;
        private Label snappingLabel;
        private Label songLengthLabel;
        private Label beatLengthLabel;
        public InfoUI(VisualElement root, SongData currentSongData)
        {
            linesLabel = root.Q<Label>("InfoLines");
            bpmLabel = root.Q<Label>("InfoBPM");
            snappingLabel = root.Q<Label>("InfoSnapping");
            songLengthLabel = root.Q<Label>("SongLength");
            beatLengthLabel = root.Q<Label>("BeatLength");

            currentSongData.OnLinesChanged += (int i) => UpdateLines(i);
            currentSongData.OnBPMChanged += (int i) => UpdateBPM(i);
            currentSongData.OnSnappingChanged += (SnappingType snap) => UpdateSnapping(snap);
            currentSongData.OnSongChanged += (AudioClip clip) => UpdateSongLength(clip.length);

            currentSongData.OnBPMChanged += (int bpm) => UpdateBeatLength(60f / bpm);

            UpdateBPM(currentSongData.BPM);
            UpdateLines(currentSongData.Lines);
            UpdateSnapping(currentSongData.Snapping);

            UpdateBeatLength(60f / currentSongData.BPM);
        }
        public void UpdateBeatLength(float? beatLength)
        {
            string lengthText = beatLength?.ToString() ?? "---";

            beatLengthLabel.text = $"Beat Duration: {lengthText} sec.";
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