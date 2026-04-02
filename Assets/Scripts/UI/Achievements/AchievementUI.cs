using KJakub.Octave.Managers.AchievementsManager;
using KJakub.Octave.ScriptableObjects;
using KJakub.Octave.UI.Core;
using System.Collections.Generic;
using UnityEngine;
namespace KJakub.Octave.UI.Achievements
{
    public class AchievementUI : MonoBehaviour
    {
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private AchievementsManager achievementsManager;
        [SerializeField]
        private GameObject prefab;
        [SerializeField]
        private Transform container;
        private void OnEnable()
        {
            GenerateAchievementItems(achievementsManager.GetUnlockedAchievements(), achievementsManager.GetLockedAchievements());
        }
        private void GenerateAchievementItems(AchievementSO[] unlockedAchievements, AchievementSO[] lockedAchievements)
        {
            List<GameObject> objectsToDelete = new();

            for (int i = 0; i < container.childCount; i++)
            {
                objectsToDelete.Add(container.GetChild(i).gameObject);
            }

            for (int i = objectsToDelete.Count - 1; i >= 0; i--)
            {
                Destroy(objectsToDelete[i]);
            }

            for (int i = 0; i < unlockedAchievements.Length; i++)
            {
                GameObject obj = Instantiate(prefab, container);
                AchievementItemUI aiUI = obj.GetComponent<AchievementItemUI>();
                AchievementSO ach = unlockedAchievements[i];
                aiUI.UpdateInfo(ach.Title, ach.Description, ach.Texture, ach.BackgroundColor, ach.ShadowColor);
                aiUI.Unlock();
            }

            for (int i = 0; i < lockedAchievements.Length; i++)
            {
                GameObject obj = Instantiate(prefab, container);
                AchievementItemUI aiUI = obj.GetComponent<AchievementItemUI>();
                AchievementSO ach = lockedAchievements[i];
                aiUI.UpdateInfo(ach.Title, ach.Description, ach.Texture, ach.BackgroundColor, ach.ShadowColor);
                aiUI.Lock();
            }
        }
        public void BackToMenu()
        {
            uiController.ShowMainMenu();
            uiController.HideAchievements();
        }
    }
}