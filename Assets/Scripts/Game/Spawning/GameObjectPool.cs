using UnityEngine;
using UnityEngine.Pool;
namespace KJakub.Octave.Game.Spawning
{
    public class GameObjectPool
    {
        GameObject prefab;
        Transform container;
        ObjectPool<GameObject> pool;
        public ObjectPool<GameObject> Pool { get { return pool; } }
        public GameObjectPool(GameObject prefab, Transform container, int defSize = 10, int maxSize = 20)
        {
            (this.prefab, this.container) = (prefab, container);

            pool = new ObjectPool<GameObject>(
                    Create,
                    OnGet,
                    OnRelease,
                    OnDestroy,
                    true,
                    defSize,
                    maxSize
                );
        }
        private GameObject Create()
        {
            var go = GameObject.Instantiate(prefab, container);
            return go;
        }
        private void OnGet(GameObject go)
        {
            go.SetActive(true);
        }
        private void OnRelease(GameObject go)
        {
            go.SetActive(false);
        }
        private void OnDestroy(GameObject go)
        {
            GameObject.Destroy(go);
        }
    }
}