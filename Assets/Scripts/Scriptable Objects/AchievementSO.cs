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
        [Header("Cosmetic")]
        [SerializeField]
        private Color backgroundColor;
        [SerializeField]
        private Color shadowColor;
        public string ID { get { return id; } }
        public Sprite Texture { get { return texture; } }
        public string Title { get { return title; } }
        public string Description { get { return description; } }
        public Color BackgroundColor { get { return backgroundColor; } }
        public Color ShadowColor { get { return shadowColor; } }
        public abstract bool IsUnlocked();
    }
}
