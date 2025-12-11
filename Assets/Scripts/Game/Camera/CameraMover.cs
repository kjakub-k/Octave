using UnityEngine;
namespace KJakub.Octave.Game.Camera
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField]
        private new UnityEngine.Camera camera;
        [SerializeField]
        private float speed = 0.7f;
        private Vector3 targetPos;
        private void Start()
        {
            if (camera == null)
                camera = GetComponent<UnityEngine.Camera>();

            targetPos = camera.transform.position;
        }
        public void UpdateCamera(float newXCoordinate)
        {
            targetPos = new Vector3(newXCoordinate, camera.transform.position.y, camera.transform.position.z);
        }
        void Update()
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, targetPos, speed * Time.deltaTime);
        }
    }
}