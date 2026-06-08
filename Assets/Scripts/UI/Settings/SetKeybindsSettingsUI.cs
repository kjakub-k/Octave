using KJakub.Octave.Data;
using KJakub.Octave.Game.Core;
using KJakub.Octave.Managers.LanguageManager;
using KJakub.Octave.Managers.SettingsManager;
using System.Collections.Generic;
using TMPro;
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
        [Header("Labels (For Translation)")]
        [SerializeField] private TMP_Text stopEditingKeybindsLabel;
        [SerializeField] private TMP_Text modeLabel;
        [SerializeField] private TMP_Text keybindLabel;
        [SerializeField] private TMP_Text keyboardLabel;
        [SerializeField] private TMP_Text controllerLabel;
        private List<GameObject> spawnedItems = new();
        private void OnEnable()
        {
            Translate();
        }
        private void Translate()
        {
            stopEditingKeybindsLabel.text = LanguageManager.GetTranslation("stop_editing_keybinds");
            modeLabel.text = LanguageManager.GetTranslation("keybinds_mode_settings");
            keybindLabel.text = LanguageManager.GetTranslation("keybind");
            keyboardLabel.text = LanguageManager.GetTranslation("keyboard");
            controllerLabel.text = LanguageManager.GetTranslation("controller");
        }
        public void OnRhythmModeDropdownChanged(int value)
        {
            int currentLaneCount = value + 1;
            settingsUI.SetLaneCount(currentLaneCount);
            BuildUI(currentLaneCount);
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

                item.laneText.text = $"{LanguageManager.GetTranslation("lane")} {i + 1}";

                RefreshItem(item, index);

                string rebindText = LanguageManager.GetTranslation("rebind");

                item.keyboardRebindBtnLabel.text = rebindText;
                item.gamepadRebindBtnLabel.text = rebindText;

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