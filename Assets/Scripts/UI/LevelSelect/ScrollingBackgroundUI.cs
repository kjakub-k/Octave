using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.AlbumSelect
{
    public class ScrollingBackgroundUI : MonoBehaviour
    {
        public float scrollSpeed = 0.1f;
        private RawImage img;

        void Start()
        {
            img = GetComponent<RawImage>();
        }

        void Update()
        {
            img.uvRect = new Rect(
                img.uvRect.x - scrollSpeed * Time.deltaTime,
                img.uvRect.y + scrollSpeed * Time.deltaTime,
                img.uvRect.width,
                img.uvRect.height
            );
        }
    }
}
