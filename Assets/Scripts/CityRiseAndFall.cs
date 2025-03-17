using UnityEngine;

public class CityRiseAndFall : MonoBehaviour
{
    [Header("Movement Settings")]
    // Total height the city should rise.
    public float targetRiseHeight = 70f;
    // Speed of rising and falling in units per second.
    public float speed = 5f;

    [Header("Timing")]
    // Time (in seconds) the city should remain at the apex.
    public float waitTimeAtApex = 10f;
    // Time (in seconds) to wait before starting the rise.
    public float initialDelay = 10f;

    // Internal tracking of how much the city has moved.
    private float currentRise = 0f;
    // Timer for the apex wait.
    private float waitTimer = 0f;
    // Timer for the initial delay.
    private float delayTimer = 0f;

    // Define the different states of the movement.
    private enum CityState { DelayedStart, Rising, Waiting, Falling, Finished }
    private CityState state = CityState.DelayedStart;

    void Update()
    {
        switch (state)
        {
            case CityState.DelayedStart:
                // Count up until we've waited 10 seconds (or whatever initialDelay is).
                delayTimer += Time.deltaTime;
                if (delayTimer >= initialDelay)
                {
                    state = CityState.Rising;
                }
                break;

            case CityState.Rising:
                // Move upward at 'speed' units per second until reaching the target height.
                float riseStep = speed * Time.deltaTime;
                if (currentRise + riseStep >= targetRiseHeight)
                {
                    riseStep = targetRiseHeight - currentRise;
                    state = CityState.Waiting; // Transition to waiting at apex
                }
                transform.position += new Vector3(0, riseStep, 0);
                currentRise += riseStep;
                break;

            case CityState.Waiting:
                // Remain at the apex for 'waitTimeAtApex' seconds.
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTimeAtApex)
                {
                    state = CityState.Falling;
                }
                break;

            case CityState.Falling:
                // Move downward at 'speed' units per second until back at original height.
                float fallStep = speed * Time.deltaTime;
                if (currentRise - fallStep <= 0f)
                {
                    fallStep = currentRise;
                    state = CityState.Finished; // Once we hit the ground, we're done
                }
                transform.position -= new Vector3(0, fallStep, 0);
                currentRise -= fallStep;
                break;

            case CityState.Finished:
                // Movement complete; do nothing else.
                break;
        }
    }
}
