using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace KJakub.Octave.Game.Core
{
    public class InputController : MonoBehaviour
    {
        public InputSystem_Actions input;
        private int currentLaneCount;
        private InputAction[] laneActions;

        private void Awake()
        {
            input = new InputSystem_Actions();

            laneActions = new InputAction[]
            {
                input.Player.Line_0,
                input.Player.Line_1,
                input.Player.Line_2,
                input.Player.Line_3,
                input.Player.Line_4,
                input.Player.Line_5,
                input.Player.Line_6
            };
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }
        public void SetLaneCount(int laneCount)
        {
            currentLaneCount = laneCount;

            for (int i = 0; i < laneActions.Length; i++)
            {
                if (laneActions[i] == null)
                {
                    Debug.LogWarning($"Lane_{i} action is missing!");
                    continue;
                }

                if (i < laneCount)
                    laneActions[i].Enable();
                else
                    laneActions[i].Disable();
            }
        }
        public void StartRebind(int index, Action onComplete = null)
        {
            var action = laneActions[index];
            if (action == null || action.bindings.Count == 0)
            {
                Debug.LogError($"Lane {index} has no binding!");
                return;
            }

            action.Disable();

            action.PerformInteractiveRebinding(0)
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnComplete(op =>
                {
                    action.Enable();
                    op.Dispose();
                    onComplete?.Invoke();
                })
                .OnCancel(op =>
                {
                    action.Enable();
                    op.Dispose();
                })
                .Start();
        }
        public string SaveCurrentLayoutRebinds()
        {
            return input.SaveBindingOverridesAsJson();
        }

        public void LoadLayoutRebinds(string json)
        {
            input.RemoveAllBindingOverrides();

            if (!string.IsNullOrEmpty(json))
                input.LoadBindingOverridesFromJson(json);
        }
        public string SaveRebinds()
        {
            return input.SaveBindingOverridesAsJson();
        }

        public void LoadRebinds(string json)
        {
            if (!string.IsNullOrEmpty(json))
                input.LoadBindingOverridesFromJson(json);
        }
        public string GetBindingName(string actionName)
        {
            var action = input.FindAction(actionName);
            return action.GetBindingDisplayString();
        }
    }
}