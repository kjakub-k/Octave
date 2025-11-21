using UnityEngine;
namespace KJakub.Octave.Game.Notes
{
    public class NoteGO : MonoBehaviour
    {
        private float speed = 10f;
        public float Speed { get { return speed; } set { speed = value; } }
        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}