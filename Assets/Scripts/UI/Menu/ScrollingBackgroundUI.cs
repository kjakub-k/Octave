using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace KJakub.Octave.UI.Menu
{
    public class ScrollingBackgroundUI
    {
        private float scrollSpeed;
        private VisualElement bg;
        private float newOffset;
        public ScrollingBackgroundUI(float scrollSpeed, VisualElement bg)
        {
            this.scrollSpeed = scrollSpeed;
            this.bg = bg;
            Move();
        }
        private void Move()
        {
            bg.schedule.Execute(() => {
                newOffset += scrollSpeed * Time.deltaTime;

                var bgPosition = new StyleBackgroundPosition
                {
                    value = new BackgroundPosition
                    {
                        offset = newOffset,
                        keyword = BackgroundPositionKeyword.Left
                    }
                };
                bg.style.backgroundPositionX = bgPosition;
                bg.style.backgroundPositionY = bgPosition;
            }).Every(1);
        }
    }
}