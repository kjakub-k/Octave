using UnityEngine;
namespace KJakub.Octave.Game.Lines
{
    public class LineManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject linePrefab;
        [SerializeField]
        private int numberOfLines = 4;
        public int LinesAmount { get { return numberOfLines; } }
        void Start()
        {
            float lineWidth = linePrefab.transform.localScale.x;
            float totalWidth = numberOfLines * lineWidth;
            float startX = -totalWidth / 2 + lineWidth / 2;
            for (int i = 0; i < numberOfLines; i++)
            {
                Vector3 pos = new Vector3(startX + i * lineWidth, 0, 0);
                Instantiate(linePrefab, pos, Quaternion.identity, transform);
            }
        }
    }
}