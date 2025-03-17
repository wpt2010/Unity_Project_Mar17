using UnityEngine;

public class SunCycle : MonoBehaviour
{
    // Total time for the sun to complete its arc (from sunrise to sunset) in seconds.
    public float duration = 40f;
    
    // This will store how many degrees to rotate per second.
    public float rotationSpeed;

    void Start()
    {
        // We want the sun to move along a 180° arc over 'duration' seconds.
        rotationSpeed = 180f / duration;
    }

    void Update()
    {
        // Rotate the pivot around its local x-axis.
        // Adjust the axis if your scene’s orientation requires a different axis.
        transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
    }
}