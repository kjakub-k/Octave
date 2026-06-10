using KJakub.Octave.Data;
using KJakub.Octave.Managers.AttemptsManager;
using KJakub.Octave.Managers.GamejoltManager;
using KJakub.Octave.Managers.LanguageManager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace KJakub.Octave.UI.LevelSelect
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField]
        private Transform container;
        [SerializeField]
        private LeaderboardItemPrefab leaderboardScorePrefab;
        [SerializeField]
        private Transform prefabContainer;
        public string LevelID { get; set; }
        public void Open()
        {
            container.gameObject.SetActive(true);
            GenerateForLocal();
        }
        public void Close()
        {
            container.gameObject.SetActive(false);
        }
        public async void GenerateForLocal()
        {
            List<int> scores = AttemptsManager.Instance.Get(LevelID, 15);

            foreach (Transform child in prefabContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var score in scores)
            {
                LeaderboardItemPrefab prefab = Instantiate(leaderboardScorePrefab, prefabContainer);
                prefab.UpdateUI(LanguageManager.GetTranslation("you"), score.ToString());
            }
        }
        public async void GenerateForFriends()
        {
            /*
            var friendsListTask = GamejoltLoader.Instance.GetFriendsList();
            var tablesTask = GamejoltLoader.Instance.GetScoreTables();

            List<int> friendIds = await friendsListTask;
            var tables = await tablesTask;

            var table = tables.FirstOrDefault(t => t["name"]?.ToString() == LevelID);

            var scores = await GamejoltLoader.Instance.GetScoresFromTable(table["id"]?.ToString());

            foreach (Transform child in prefabContainer)
            {
                Destroy(child.gameObject);
            }

            string loggedInUser = PlayerPrefs.GetString("username", "").ToLower();

            var filteredScores = scores.Where(entry =>
            {
                string entryUser = entry["user"]?.ToString().ToLower();

                if (!string.IsNullOrEmpty(entryUser) && entryUser == loggedInUser)
                {
                    return true;
                }

                if (entry["user_id"] != null && int.TryParse(entry["user_id"].ToString(), out int entryUserId))
                {
                    return friendIds.Contains(entryUserId);
                }

                return false;
            }).ToList();

            foreach (var entry in filteredScores)
            {
                LeaderboardItemPrefab prefab = Instantiate(leaderboardScorePrefab, prefabContainer);

                string playerName = entry["user"]?.ToString();
                if (string.IsNullOrEmpty(playerName))
                {
                    playerName = entry["guest"]?.ToString();
                }

                string scoreText = entry["score"]?.ToString();

                prefab.UpdateUI(playerName, scoreText);
            }
            */
        }
        public async void GenerateForGlobal()
        {
            /*
            var tables = await GamejoltLoader.Instance.GetScoreTables();

            var table = tables.First(table => table["name"]?.ToString() == LevelID);

            var scores = await GamejoltLoader.Instance.GetScoresFromTable(table["id"]?.ToString());

            if (scores == null || scores.Count == 0)
            {
                return;
            }

            foreach (Transform child in prefabContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var entry in scores)
            {
                LeaderboardItemPrefab prefab = Instantiate(leaderboardScorePrefab, prefabContainer);

                string playerName = entry["user"]?.ToString();

                if (string.IsNullOrEmpty(playerName))
                {
                    playerName = entry["guest"]?.ToString();
                }

                string scoreText = entry["score"]?.ToString();

                prefab.UpdateUI(playerName, scoreText);
            }
            */
        }
    }
}