using KJakub.Octave.Game.Lines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace KJakub.Octave.Game.Core
{
    enum PressRecorderStatus
    {
        Recording,
        Inactive,
        PlayingPresses
    }
    public class PressRecorder : MonoBehaviour
    {
        private PressRecorderStatus status = PressRecorderStatus.Inactive;
        public event Action<float, int> OnPress;
        public IEnumerator DoPresses(NoteDetector[] detectors, List<(float, int)?> presses)
        {
            status = PressRecorderStatus.PlayingPresses;
            var shallowPresses = new List<(float, int)?>(presses);

            double playbackStartTime = Time.realtimeSinceStartupAsDouble;

            while (status == PressRecorderStatus.PlayingPresses)
            {
                double currentTime = Time.realtimeSinceStartupAsDouble - playbackStartTime;

                for (int i = shallowPresses.Count - 1; i >= 0; i--)
                {
                    var press = shallowPresses[i];

                    if (press.HasValue && press.Value.Item1 <= currentTime)
                    {
                        int index = press.Value.Item2;

                        detectors[index].OnNoteDetectorPress();
                        shallowPresses.RemoveAt(i);

                        yield return null;

                        detectors[index].ChangeMaterial(0);
                    }
                }

                if (shallowPresses.Count == 0)
                    status = PressRecorderStatus.Inactive;

                yield return null;
            }

            status = PressRecorderStatus.Inactive;
        }
        private double recordingStartTime;
        public void StartRecording(PlayerInput playerInput, int linesAmount)
        {
            status = PressRecorderStatus.Recording;
            recordingStartTime = Time.timeAsDouble;

            for (int i = 0; i < linesAmount; i++)
            {
                int index = i;
                var action = playerInput.actions.FindAction($"Line_{linesAmount - i - 1}");

                action.performed += ctx =>
                {
                    if (status != PressRecorderStatus.Recording) 
                        return;

                    double relativeTime = Time.timeAsDouble - recordingStartTime;
                    OnPress?.Invoke((float)relativeTime, index);
                };
            }
        }
        public void Stop()
        {
            status = PressRecorderStatus.Inactive;
        }
    }
}