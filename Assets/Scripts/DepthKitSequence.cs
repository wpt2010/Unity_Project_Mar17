using UnityEngine;
using System.Collections;
using Cinemachine;

public class DepthKitSequence : MonoBehaviour
{
    // Array of your depth kit clip containers (children of a single parent).
    public GameObject[] depthKitClips;
    
    // The base distance (in local units) each clip should move toward the camera.
    public float moveDistance = 15f;
    // Movement speed (local units per second).
    public float speed = 1f;
    // Duration (in seconds) of the crossfade transition.
    public float fadeDuration = 1f;
    
    // Reference to the Cinemachine Virtual Camera that orbits around the clips.
    public CinemachineVirtualCamera orbitCam;
    
    // Index of the current clip being animated.
    private int currentIndex = 0;
    // Record the starting local z position of the current clip.
    private float startZ;
    // Flag to prevent multiple simultaneous transitions.
    private bool isSwitching = false;
    
    void Start()
    {
        // Verify that clips have been assigned.
        if (depthKitClips == null || depthKitClips.Length == 0)
        {
            Debug.LogError("No depth kit clips assigned in the DepthKitSequence script.");
            return;
        }
        
        // Activate only the first clip; deactivate the rest.
        for (int i = 0; i < depthKitClips.Length; i++)
        {
            depthKitClips[i].SetActive(i == 0);
            // Set alpha: first clip fully visible, others fully transparent.
            SetAlpha(depthKitClips[i], (i == 0) ? 1f : 0f);
        }
        
        // Record the starting local z position of the first clip.
        startZ = depthKitClips[currentIndex].transform.localPosition.z;
        
        // Update the orbit camera's Follow and LookAt targets to the first clip.
        if (orbitCam != null)
        {
            orbitCam.Follow = depthKitClips[currentIndex].transform;
            orbitCam.LookAt = depthKitClips[currentIndex].transform;
        }
    }
    
    void Update()
    {
        // If all clips have finished or we are already switching, do nothing.
        if (currentIndex >= depthKitClips.Length || isSwitching)
            return;
        
        // Get the current active clip.
        GameObject currentClip = depthKitClips[currentIndex];
        
        // Calculate the movement for this frame.
        float moveStep = speed * Time.deltaTime;
        // Move the clip in the negative local z direction (simulate "walking" toward the camera).
        currentClip.transform.localPosition += new Vector3(0, 0, -moveStep);
        
        // Calculate how far the clip has moved (using local z positions).
        float movedDistance = startZ - currentClip.transform.localPosition.z;
        
        // Determine the target distance for this clip.
        // For the second clip (index 1), reduce the distance by 3 units.
        float currentTargetDistance = (currentIndex == 1) ? moveDistance - 3f : moveDistance;
        
        // When the current clip has moved the target distance, trigger the transition.
        if (movedDistance >= currentTargetDistance)
        {
            // Only start fade transition if there is a next clip.
            if (currentIndex + 1 < depthKitClips.Length)
            {
                StartCoroutine(FadeTransition(currentClip, depthKitClips[currentIndex + 1]));
            }
            else
            {
                // If it's the last clip, you can optionally end the sequence.
            }
        }
    }
    
    IEnumerator FadeTransition(GameObject currentClip, GameObject nextClip)
    {
        isSwitching = true;
        
        // Position the next clip exactly at the current clip's local position.
        nextClip.transform.localPosition = currentClip.transform.localPosition;
        nextClip.SetActive(true);
        
        // Update the Cinemachine camera's Follow and LookAt target to the next clip.
        if (orbitCam != null)
        {
            orbitCam.Follow = nextClip.transform;
            orbitCam.LookAt = nextClip.transform;
        }
        
        // Get the Renderer components.
        Renderer currentRenderer = currentClip.GetComponent<Renderer>();
        Renderer nextRenderer = nextClip.GetComponent<Renderer>();
        
        if (currentRenderer == null || nextRenderer == null)
        {
            Debug.LogError("Missing Renderer on one of the clips. Fade cannot be performed.");
            yield break;
        }
        
        // Set next clip's alpha to 0 before starting the fade.
        SetAlpha(nextClip, 0f);
        
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alphaCurrent = Mathf.Lerp(1f, 0f, t / fadeDuration);
            float alphaNext = Mathf.Lerp(0f, 1f, t / fadeDuration);
            
            SetAlpha(currentClip, alphaCurrent);
            SetAlpha(nextClip, alphaNext);
            
            yield return null;
        }
        
        // Ensure final alpha values.
        SetAlpha(currentClip, 0f);
        SetAlpha(nextClip, 1f);
        
        // Deactivate the current clip.
        currentClip.SetActive(false);
        
        // Advance to the next clip.
        currentIndex++;
        if (currentIndex < depthKitClips.Length)
        {
            startZ = depthKitClips[currentIndex].transform.localPosition.z;
        }
        isSwitching = false;
    }
    
    // Helper method to set the alpha of the first Renderer found on the clip.
    void SetAlpha(GameObject clip, float alpha)
    {
        Renderer rend = clip.GetComponent<Renderer>();
        if (rend != null)
        {
            Color col = rend.material.color;
            col.a = alpha;
            rend.material.color = col;
        }
    }
}

