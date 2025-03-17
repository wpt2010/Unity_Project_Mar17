using UnityEngine;
using Cinemachine;

public class AutomatedOrbit : MonoBehaviour
{
    // Orbit speed in degrees per second.
    public float orbitSpeed = 10f;

    private CinemachineOrbitalTransposer orbitalTransposer;

    void Start()
    {
        // Get the Cinemachine Virtual Camera component on this GameObject.
        CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            // Get the Orbital Transposer component from the vcam's Body.
            orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            if (orbitalTransposer == null)
            {
                Debug.LogError("No CinemachineOrbitalTransposer found. Make sure the Body is set to Orbital Transposer.");
            }
            else
            {
                // Disable mouse input.
                orbitalTransposer.m_XAxis.m_InputAxisName = "";

                // Enable wrapping and set a wide axis range.
                var axis = orbitalTransposer.m_XAxis;
                axis.m_Wrap = true;
                axis.m_MinValue = -9999f;
                axis.m_MaxValue = 9999f;
                
                // Disable auto-recentering so the orbit isn't reset.
                axis.m_Recentering.m_enabled = false;

                orbitalTransposer.m_XAxis = axis;
            }
        }
        else
        {
            Debug.LogError("No CinemachineVirtualCamera component found on this GameObject.");
        }
    }

    void Update()
    {
        if (orbitalTransposer != null)
        {
            // Continuously increment the orbital X-axis value for a full orbit.
            orbitalTransposer.m_XAxis.Value += orbitSpeed * Time.deltaTime;
        }
    }
}