using UnityEngine;
namespace KJakub.Octave.Game.Notes
{
    public class NoteGO : MonoBehaviour
    {
        public float speed = 5f;
        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}