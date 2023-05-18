using UnityEngine;

namespace Infrastructure.WeaponManagment
{
    [RequireComponent(typeof(LineRenderer))]
    public class DrawRing : MonoBehaviour
    {
        public float radius = 5.0f;
        public int segments = 32;
        public Color color = Color.yellow;

        private LineRenderer lineRenderer;

        private void Start()
        {
            // Set up line renderer
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = segments + 1;
            lineRenderer.useWorldSpace = false;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            // color of line
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.material.color = color;

            // positions in the circle
            Vector3[] positions = new Vector3[segments + 1];
            float angle = 0f;
            float angleStep = 2f * Mathf.PI / segments;

            for (int i = 0; i < segments + 1; i++)
            {
                positions[i] = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
                angle += angleStep;
            }

            // set up positions to renderer
            lineRenderer.SetPositions(positions);
        }
    }
}