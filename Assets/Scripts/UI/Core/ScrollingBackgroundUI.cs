using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Core
{
    public class ScrollingBackgroundUI : MonoBehaviour
    {
        public float scrollSpeedX = 0.1f;
        public float scrollSpeedY = 0.1f;
        private RawImage img;
        void Start()
        {
            img = GetComponent<RawImage>();
        }
        void Update()
        {
            img.uvRect = new Rect(
                img.uvRect.x - scrollSpeedX * Time.deltaTime,
                img.uvRect.y + scrollSpeedY * Time.deltaTime,
                img.uvRect.width,
                img.uvRect.height
            );
        }
    }
}