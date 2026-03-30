using KJakub.Octave.Data;
using KJakub.Octave.Game.Core;
using KJakub.Octave.Managers.SettingsManager;
using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.UI.Settings
{
    public class SetKeybindsSettingsUI : MonoBehaviour
    {
        [SerializeField] private GameObject scrollView;
        [SerializeField] private GameObject otherScrollView;
        [SerializeField] private InputController inputController;
        [SerializeField] private Transform lanesContainer;
        [SerializeField] private GameObject laneItemPrefab;
        [SerializeField] private SettingsUI settingsUI;
        [SerializeField] private SettingsManager settingsManager;

        private List<GameObject> spawnedItems = new();
        public void OnRhythmModeDropdownChanged(int value)
        {
            int currentLaneCount = value + 1;

            settingsUI.SetLaneCount(currentLaneCount);

            var profile = settingsManager.CurrentProfile;

            inputController.ClearAllBindings();

            if (profile.RebindsByLaneCount.TryGetValue(currentLaneCount, out InputBindingSet set))
            {
                inputController.LoadBindingGroup(set.KeyboardJSON);
                inputController.LoadBindingGroup(set.GamepadJSON);
            }
        }
        public void Open()
        {
            scrollView.SetActive(true);
            otherScrollView.SetActive(false);
            BuildUI(settingsUI.GetLaneCount());
        }
        public void Close()
        {
            scrollView.SetActive(false);
            otherScrollView.SetActive(true);
        }
        public void RebindLineKeyboard(int index)
        {
            inputController.StartRebind(index, InputDeviceType.Keyboard, RefreshUI);
        }
        public void RebindLineGamepad(int index)
        {
            inputController.StartRebind(index, InputDeviceType.Gamepad, RefreshUI);
        }
        public void BuildUI(int laneCount)
        {
            foreach (var go in spawnedItems)
                Destroy(go);

            spawnedItems.Clear();

            inputController.SetLaneCount(laneCount);

            var profile = settingsManager.CurrentProfile;

            inputController.ClearAllBindings();

            if (profile.RebindsByLaneCount.TryGetValue(laneCount, out InputBindingSet set))
            {
                inputController.LoadBindingGroup(set.KeyboardJSON);
                inputController.LoadBindingGroup(set.GamepadJSON);
            }

            for (int i = 0; i < laneCount; i++)
            {
                int index = i;

                var go = Instantiate(laneItemPrefab, lanesContainer);
                var item = go.GetComponent<LaneItemPrefab>();

                item.laneText.text = $"Lane {i + 1}";

                RefreshItem(item, index);

                item.keyboardRebindButton.onClick.AddListener(() =>
                {
                    inputController.StartRebind(index, InputDeviceType.Keyboard, () =>
                    {
                        SaveBindings();
                        RefreshUI();
                    });
                });

                item.gamepadRebindButton.onClick.AddListener(() =>
                {
                    inputController.StartRebind(index, InputDeviceType.Gamepad, () =>
                    {
                        SaveBindings();
                        RefreshUI();
                    });
                });

                spawnedItems.Add(go);
            }
        }

        private void RefreshItem(LaneItemPrefab item, int index)
        {
            item.keyboardText.text =
                inputController.GetBindingName($"Line_{index}", InputDeviceType.Keyboard);

            item.gamepadText.text =
                inputController.GetBindingName($"Line_{index}", InputDeviceType.Gamepad);
        }

        private void RefreshUI()
        {
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                var item = spawnedItems[i].GetComponent<LaneItemPrefab>();
                RefreshItem(item, i);
            }
        }

        private void SaveBindings()
        {
            int laneCount = settingsUI.GetLaneCount();
            var profile = settingsManager.CurrentProfile;

            if (!profile.RebindsByLaneCount.ContainsKey(laneCount))
                profile.RebindsByLaneCount[laneCount] = new InputBindingSet();

            InputBindingSet set = profile.RebindsByLaneCount[laneCount];

            set.KeyboardJSON = inputController.SaveBindingGroup(InputDeviceType.Keyboard);
            set.GamepadJSON = inputController.SaveBindingGroup(InputDeviceType.Gamepad);

            settingsManager.SaveSettingsToJSON();
        }
    }
}