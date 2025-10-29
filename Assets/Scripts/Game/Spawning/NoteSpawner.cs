using KJakub.Octave.Game.Lines;
using UnityEngine;
namespace KJakub.Octave.Game.Spawning
{
    public class NoteSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject notePrefab;
        float timer = 1f;
        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            } else if (timer < 0)
            {
                SpawnNote(Random.Range(0, GetComponent<LineManager>().LinesAmount));
                timer = 1f;
            }
        }
        public void SpawnNote(int lineIndex)
        {
            Transform line = transform.GetChild(lineIndex);
            Instantiate(notePrefab, line.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }
}