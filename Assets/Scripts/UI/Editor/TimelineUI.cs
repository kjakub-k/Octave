using KJakub.Octave.Data;
using KJakub.Octave.Managers.CommandManager;
using KJakub.Octave.Managers.CommandManager.NoteCommandManager;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
namespace KJakub.Octave.UI.Editor
{
    public class TimelineUI
    {
        private ScrollView linesView;
        private VisualElement lanesContainer;
        private NoteCommandManager commandManager;
        private SongData currentSongData;
        private int pixelsPerSec = 300;
        private float songLength = 20; //this is in seconds
        public TimelineUI(VisualElement root, SongData songData, NoteCommandManager commandManager)
        {
            this.commandManager = commandManager;
            currentSongData = songData;

            linesView = root.Q<ScrollView>("LinesView");
            lanesContainer = linesView.Q<VisualElement>("LanesContainer");
            
            CreateLanes(songData.Lines, songData.BPM, songData.Snapping);

            songData.OnLinesChanged += (int i) => CreateLanes(i, songData.BPM, songData.Snapping);
            songData.OnBPMChanged += (int i) => CreateLanes(songData.Lines, i, songData.Snapping);
            songData.OnSnappingChanged += (SnappingType snap) => CreateLanes(songData.Lines, songData.BPM, snap);
            songData.OnSongChanged += ChangeSongLength;
            songData.OnSongUpdated += () => ChangeSongLength(songData.Song);
            songData.OnSongUpdated += RenderNotes;
        }
        private void CreateLanes(int lanes, int bPM, SnappingType snapping)
        {
            lanesContainer.Clear();

            float secondsPerBeat = 60f / bPM;
            int beatPixelsLength = Mathf.RoundToInt(secondsPerBeat * pixelsPerSec);
            int totalBeats = Mathf.CeilToInt(songLength / secondsPerBeat);

            for (int i = 0; i < lanes; i++)
            {
                VisualElement lane = new VisualElement();
                lane.AddToClassList("lane");
                lane.style.height = Length.Pixels(songLength * pixelsPerSec);

                CreateBeats(lane, beatPixelsLength, totalBeats, snapping, i);
                lanesContainer.Add(lane);
            }

            linesView.schedule.Execute(() =>
            {
                linesView.verticalScroller.value = linesView.verticalScroller.highValue;
            });
        }
        private void CreateBeats(VisualElement lane, int beatPixelsLength, int totalBeats, SnappingType snapping, int laneIndex)
        {
            for (int i = 0; i < totalBeats; i++)
            {
                VisualElement beat = new();
                beat.AddToClassList("beat");
                beat.style.height = Length.Pixels(beatPixelsLength);

                for (int j = 0; j < (int)snapping; j++)
                {
                    VisualElement noteElement = new();
                    VisualElement noteType = new();
                    noteType.style.height = Length.Percent(100);
                    noteType.style.width = Length.Percent(100);

                    float beatTime = i * (60f / currentSongData.BPM) + j * (60f / currentSongData.BPM / (int)snapping);
                    NoteData note = new() { Lane = laneIndex, Time = beatTime };

                    noteType.RegisterCallback<PointerUpEvent>(evt =>
                    {
                        if (evt.button == 0)
                        {
                            if (currentSongData.Notes.Where(n => n.Time == note.Time && n.Lane == note.Lane).Any())
                                return;

                            AddNoteCommand cmd = new(note, currentSongData.Notes, noteType);
                            commandManager.Execute(cmd);
                        } else if (evt.button == 1)
                        {
                            RemoveNoteCommand cmd = new(note, currentSongData.Notes, noteType);
                            commandManager.Execute(cmd);
                        }
                    });

                    noteElement.AddToClassList("note");

                    //purely for cosmetic purposes, so the editor could easily distinguish beats between each other
                    if (j == (int)snapping - 1)
                        noteElement.AddToClassList("last");

                    noteElement.Add(noteType);
                    beat.Add(noteElement);
                }

                lane.Add(beat);
            }
        }
        private void ChangeSongLength(AudioClip clip)
        {
            songLength = clip.length;
            CreateLanes(currentSongData.Lines, currentSongData.BPM, currentSongData.Snapping);
        }
        private void RenderNotes()
        {
            foreach (var note in currentSongData.Notes)
            {
                var lane = lanesContainer.ElementAt(note.Lane);

                if (lane == null) 
                    continue;

                float secondsPerBeat = 60f / currentSongData.BPM;
                int totalBeats = Mathf.FloorToInt(note.Time / secondsPerBeat);
                float beatOffset = note.Time % secondsPerBeat;

                int snapDivisions = (int)currentSongData.Snapping;
                float snapLength = secondsPerBeat / snapDivisions;
                int snapIndex = Mathf.RoundToInt(beatOffset / snapLength);

                snapIndex = Mathf.Clamp(snapIndex, 0, snapDivisions - 1);
                totalBeats = Mathf.Clamp(totalBeats, 0, lane.childCount - 1);

                var beat = lane.ElementAt(totalBeats);

                if (beat == null) 
                    continue;

                var noteElement = beat.ElementAt(snapIndex);

                if (noteElement == null) 
                    continue;

                var noteType = noteElement.ElementAt(0);
                noteType.AddToClassList("classic");
            }
        }
    }
}