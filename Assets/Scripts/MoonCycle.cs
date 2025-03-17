using UnityEngine;

public class MoonCycle : MonoBehaviour
{
    // Time delay in seconds before the moon appears.
    public float startDelay = 32f;
    // Total time (in seconds) for the moon to complete its arc.
    public float duration = 40f;
    // Total degrees to rotate along the chosen axis (e.g., 180 for half a circle).
    public float rotationDegrees = 180f;
    
    // Calculated rotation speed (degrees per second).
    private float rotationSpeed;
    // Timer to track elapsed time.
    private float timer = 0f;
    // Flag to indicate if the moon movement has started.
    private bool started = false;

    void Start()
    {
        // Calculate the rotation speed based on desired duration.
        rotationSpeed = rotationDegrees / duration;
        // Initially disable the moon light (or its pivot) so it isn't visible.
        gameObject.SetActive(false);
    }

    void Update()
    {
        // Update the timer.
        timer += Time.deltaTime;

        // Once the timer reaches startDelay, activate the moon.
        if (!started && timer >= startDelay)
        {
            gameObject.SetActive(true);
            started = true;
        }

        // Once activated, move the moon along its arc.
        if (started)
        {
            // Rotate around the chosen axis.
            // Change the axis values below to alter the moon's path.
            transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
        }
    }
}