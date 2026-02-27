using KJakub.Octave.Game.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
namespace KJakub.Octave.UI.Settings
{
    public class SetKeybindsSettingsUI : MonoBehaviour
    {
        [SerializeField] private GameObject scrollView;
        [SerializeField] private GameObject otherScrollView;
        [SerializeField] private InputController inputController;
        [SerializeField] private Transform lanesContainer;
        [SerializeField] private GameObject laneItemPrefab;
        [SerializeField] private int currentLaneCount = 4;

        private List<GameObject> spawnedItems = new();
        public void Open()
        {
            scrollView.SetActive(true);
            otherScrollView.SetActive(false);
            BuildUI(currentLaneCount);
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
            for (int i = 0; i < currentLaneCount; i++)
            {
                var item = spawnedItems[i].GetComponent<LaneItemPrefab>();
                item.keyText.text =
                    inputController.GetBindingName($"Line_{i}");
            }
        }
        public void BuildUI(int laneCount)
        {
            currentLaneCount = laneCount;

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
                item.keyText.text = inputController.GetBindingName($"Line_{i}");

                item.rebindButton.onClick.AddListener(() =>
                {
                    inputController.StartRebind(index, () =>
                    {
                        RefreshUI();
                    });
                });

                spawnedItems.Add(go);
            }
        }
    }
}