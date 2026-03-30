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
            if (profile.RebindsByLaneCount.TryGetValue(currentLaneCount, out string json))
                inputController.LoadLayoutRebinds(json);
            else
                inputController.LoadLayoutRebinds(null);

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
        public void RebindLine(int index)
        {
            inputController.StartRebind(index, RefreshUI);
        }
        private void RefreshUI()
        {
            for (int i = 0; i < settingsUI.GetLaneCount(); i++)
            {
                var item = spawnedItems[i].GetComponent<LaneItemPrefab>();
                item.keyText.text =
                    inputController.GetBindingName($"Line_{i}");
            }
        }
        public void BuildUI(int laneCount)
        {
            foreach (var go in spawnedItems)
                Destroy(go);

            spawnedItems.Clear();

            inputController.SetLaneCount(laneCount);

            for (int i = 0; i < laneCount; i++)
            {
                int index = i;

                var go = Instantiate(laneItemPrefab, lanesContainer);
                var item = go.GetComponent<LaneItemPrefab>();

                item.laneText.text = $"Lane {i + 1}";
                
                var profile = settingsManager.CurrentProfile;
                int laneCount2 = settingsUI.GetLaneCount();
                if (profile.RebindsByLaneCount.TryGetValue(laneCount, out string json))
                {
                    inputController.LoadLayoutRebinds(json);
                }

                item.keyText.text = inputController.GetBindingName($"Line_{i}");

                item.rebindButton.onClick.AddListener(() =>
                {
                    inputController.StartRebind(index, () =>
                    {
                        int laneCount = settingsUI.GetLaneCount();
                        var profile = settingsManager.CurrentProfile;
                        profile.RebindsByLaneCount[laneCount] = inputController.SaveCurrentLayoutRebinds();
                        settingsManager.SaveSettingsToJSON();

                        RefreshUI();
                    });
                });

                spawnedItems.Add(go);
            }
        }
    }
}