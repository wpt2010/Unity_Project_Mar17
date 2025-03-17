using UnityEngine;

public class CubeMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;                // Speed of movement along x-axis
    public float endX = 30f;                // x position at which the cube is destroyed

    [Header("Spawn Settings")]
    public Vector3 startPosition = new Vector3(-30f, 8f, -7f);
    public GameObject cubePrefab;           // Assign your cube prefab here

    void Start()
    {
        // Ensure the cube starts at the designated starting position
        transform.position = startPosition;
    }

    void Update()
    {
        // Move the cube to the right (along the x-axis)
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Check if the cube has reached or passed the end position on the x-axis
        if (transform.position.x >= endX)
        {
            // Spawn a new cube at the starting position if a prefab is provided
            if (cubePrefab != null)
            {
                Instantiate(cubePrefab, startPosition, Quaternion.identity);
            }
            // Destroy this cube
            Destroy(gameObject);
        }
    }
}