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
            List<(float, int)?> shallowPresses = new(presses);
            float timer = 0;

            while (status == PressRecorderStatus.PlayingPresses)
            {
                timer += Time.deltaTime;

                for (int i = shallowPresses.Count - 1; i >= 0; i--)
                {
                    var press = shallowPresses[i];

                    if (press.Value.Item1 <= timer)
                    {
                        detectors[press.Value.Item2].OnNoteDetectorPress();
                        shallowPresses.Remove(press);
                        yield return null;
                        detectors[press.Value.Item2].ChangeMaterial(0);
                    }
                }

                if (shallowPresses.Count <= 0)
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

                    double relativeTime = ctx.time - recordingStartTime;
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