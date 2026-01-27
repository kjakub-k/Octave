using KJakub.Octave.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace KJakub.Octave.UI.Results
{
    public class ResultsGraphUI : MonoBehaviour
    {
        [Header("Graph Settings")]
        [SerializeField]
        private float minY = -10f;
        [SerializeField]
        private float maxY = 10f;
        [SerializeField]
        private RectTransform graphRect;
        [SerializeField]
        private Image dotPrefab;
        [SerializeField]
        private Image linePrefab;
        [SerializeField]
        private float lineThickness = 2f;
        public void DrawOnGraph(List<AccuracyResult> hitAccuracies, float songLength)
        {
            foreach (Transform child in graphRect)
                Destroy(child.gameObject);

            Vector2 graphSize = graphRect.rect.size;

            hitAccuracies.Sort((a, b) => a.TimeHitInSeconds.CompareTo(b.TimeHitInSeconds));

            List<Vector2> points = new List<Vector2>();

            foreach (var hit in hitAccuracies)
            {
                float x01 = Mathf.Clamp01(hit.TimeHitInSeconds / songLength);
                float y01 = Mathf.InverseLerp(minY, maxY, hit.Weight);
                y01 = 1f - y01;

                Vector2 point = new Vector2(
                    x01 * graphSize.x,
                    y01 * graphSize.y
                );

                points.Add(point);

                Image dot = Instantiate(dotPrefab, graphRect);
                RectTransform dotRect = dot.rectTransform;

                dotRect.anchorMin = Vector2.zero;
                dotRect.anchorMax = Vector2.zero;
                dotRect.pivot = new Vector2(0.5f, 0.5f);
                dotRect.anchoredPosition = point;
                dotRect.localScale = Vector3.one;

                dot.color = hit.Color;
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                DrawLine(points[i], points[i + 1]);
            }
        }
        private void DrawLine(Vector2 start, Vector2 end)
        {
            Image line = Instantiate(linePrefab, graphRect);
            line.transform.SetAsFirstSibling();

            RectTransform rect = line.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.pivot = new Vector2(0f, 0.5f);

            Vector2 dir = end - start;
            float length = dir.magnitude;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            rect.sizeDelta = new Vector2(length, lineThickness);
            rect.anchoredPosition = start;
            rect.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}