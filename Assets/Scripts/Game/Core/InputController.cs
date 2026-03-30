using KJakub.Octave.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace KJakub.Octave.Game.Core
{
    public enum InputDeviceType
    { 
        Keyboard,
        Gamepad
    }
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
        private void OnEnable() => input.Enable();
        private void OnDisable() => input.Disable();
        public void SetLaneCount(int laneCount)
        {
            currentLaneCount = laneCount;

            for (int i = 0; i < laneActions.Length; i++)
            {
                if (i < laneCount) laneActions[i].Enable();
                else laneActions[i].Disable();
            }
        }
        public void StartRebind(int index, InputDeviceType deviceType, Action onComplete = null)
        {
            var action = laneActions[index];

            int bindingIndex = GetBindingIndexForDevice(action, deviceType);

            if (bindingIndex == -1)
            {
                Debug.LogError($"No binding for {deviceType} on Lane {index}");
                return;
            }

            action.Disable();

            var rebind = action.PerformInteractiveRebinding(bindingIndex)
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape");

            if (deviceType == InputDeviceType.Keyboard)
                rebind.WithControlsHavingToMatchPath("<Keyboard>");
            else
                rebind.WithControlsHavingToMatchPath("<Gamepad>");

            rebind
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
        private int GetBindingIndexForDevice(InputAction action, InputDeviceType deviceType)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];
                if (binding.isComposite) continue;

                if (deviceType == InputDeviceType.Keyboard && binding.path.Contains("<Keyboard>"))
                    return i;
                if (deviceType == InputDeviceType.Gamepad && binding.path.Contains("<Gamepad>"))
                    return i;
            }
            return -1;
        }
        public string SaveBindingGroup(InputDeviceType deviceType)
        {
            var overrides = new List<BindingOverrideData>();

            string deviceKeyword = deviceType == InputDeviceType.Keyboard ? "<Keyboard>" : "<Gamepad>";

            foreach (var map in input.asset.actionMaps)
            {
                foreach (var action in map.actions)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        var binding = action.bindings[i];

                        if (binding.isComposite) continue;
                        if (!binding.path.Contains(deviceKeyword)) continue;

                        if (!string.IsNullOrEmpty(binding.overridePath))
                        {
                            overrides.Add(new BindingOverrideData
                            {
                                action = action.name,
                                bindingIndex = i,
                                overridePath = binding.overridePath
                            });
                        }
                    }
                }
            }

            return JsonUtility.ToJson(new BindingOverrideWrapper { list = overrides });
        }
        public void LoadBindingGroup(string json)
        {
            if (string.IsNullOrEmpty(json))
                return;

            var wrapper = JsonUtility.FromJson<BindingOverrideWrapper>(json);

            foreach (var data in wrapper.list)
            {
                var action = input.FindAction(data.action);
                if (action != null && data.bindingIndex < action.bindings.Count)
                {
                    action.ApplyBindingOverride(data.bindingIndex, data.overridePath);
                }
            }
        }
        public void ClearAllBindings()
        {
            input.RemoveAllBindingOverrides();
        }
        public string GetBindingName(string actionName, InputDeviceType deviceType)
        {
            var action = input.FindAction(actionName);

            int bindingIndex = GetBindingIndexForDevice(action, deviceType);

            if (bindingIndex == -1)
                return "Unbound";

            return action.GetBindingDisplayString(bindingIndex);
        }
    }
}