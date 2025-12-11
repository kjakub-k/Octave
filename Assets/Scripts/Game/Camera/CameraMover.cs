using DG.Tweening;
using UnityEngine;
namespace KJakub.Octave.Game.Camera
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField]
        private new UnityEngine.Camera camera;
        [SerializeField]
        private float duration = 0.7f;
        private void Start()
        {
            if (camera == null)
                camera = GetComponent<UnityEngine.Camera>();
        }
        public void UpdateCamera(float newXCoordinate)
        {
            Vector3 targetPos = new(newXCoordinate, camera.transform.position.y, camera.transform.position.z);
            camera.transform.DOMove(targetPos, duration).SetEase(Ease.InOutCirc);
        }
    }
}