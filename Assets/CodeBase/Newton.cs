using UnityEngine;

public class Newton : MonoBehaviour
{
    public Transform[] spheres;
    public float swingForce = 100f;
    public float radius = 2f;
    public float dropSpeed = 2f;

    private Vector3[] initialPositions;
    private int currentSwingIndex = 0;
    private float swingTime = 0f;
    private bool dropping = false;
    private float dropTime = 0f;
    private float dropDuration = 0f;
    private Vector3 dropStartPosition;
    private Vector3 dropEndPosition;

    void Start()
    {
        initialPositions = new Vector3[spheres.Length];
        for (int i = 0; i < spheres.Length; i++)
        {
            initialPositions[i] = spheres[i].position;
        }
    }

    void Update()
    {
        // Calculate the swing angle for the current swinging sphere
        float angle = (Time.time - swingTime) * swingForce;

        if (dropping)
        {
            // Calculate the drop progress
            float dropProgress = Mathf.Clamp01((Time.time - dropTime) / dropDuration);

            // Move the dropping sphere along the drop curve
            spheres[currentSwingIndex].position = Vector3.Lerp(dropStartPosition, dropEndPosition, dropProgress);

            // Check if the dropping sphere has reached the end of the drop curve
            if (dropProgress >= 1f)
            {
                dropping = false;

                // Stop the dropping sphere and start the next swinging sphere
                spheres[currentSwingIndex].position = dropEndPosition;
                currentSwingIndex = (currentSwingIndex + 1) % spheres.Length;
                swingTime = Time.time;
            }
        }
        else
        {
            // Move the current swinging sphere along the circumference of the swing circle
            spheres[currentSwingIndex].position = initialPositions[currentSwingIndex] + new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * radius;

            // Check if the current swinging sphere has reached the end of the swing arc
            if (angle >= Mathf.PI && !dropping)
            {
                // Start the drop of the swinging sphere
                dropping = true;
                dropTime = Time.time;
                dropDuration = Vector3.Distance(spheres[currentSwingIndex].position, initialPositions[0]) / dropSpeed;
                dropStartPosition = spheres[currentSwingIndex].position;
                dropEndPosition = initialPositions[0];

                // Reset the previous swinging sphere to its initial position
                if (currentSwingIndex > 0)
                {
                    spheres[currentSwingIndex - 1].position = initialPositions[currentSwingIndex - 1];
                }
            }
        }
    }
}