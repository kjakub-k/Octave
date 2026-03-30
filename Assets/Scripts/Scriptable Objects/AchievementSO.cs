using UnityEngine;
namespace KJakub.Octave.ScriptableObjects
{
    public abstract class AchievementSO : ScriptableObject
    {
        [SerializeField]
        private string id;
        [SerializeField]
        private Sprite texture;
        [SerializeField]
        private string title;
        [SerializeField]
        private string description;
        public string ID { get { return id; } }
        public Sprite Texture { get { return texture; } }
        public string Title { get { return title; } }
        public string Description { get { return description; } }
        public abstract bool IsUnlocked();
    }
}
