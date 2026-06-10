using TMPro;
using UnityEngine;
namespace KJakub.Octave.UI.LevelSelect 
{ 
    public class LeaderboardItemPrefab : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text nameLabel;
        [SerializeField]
        private TMP_Text scoreLabel;
        public void UpdateUI(string name, string score)
        {
            nameLabel.text = name;
            scoreLabel.text = score;
        }
    }
}