using UnityEngine;

public class LightLookAt : MonoBehaviour
{
    [Header("Targets")]
    // The big ball everyone looks at
    public Transform sharedTarget;
    // The secret small ball just for this light
    public Transform personalTarget;

    // The target it is looking at right now
    private Transform currentTarget;

    void Start()
    {
        // Start with the big ball
        currentTarget = sharedTarget;
    }

    void LateUpdate()
    {
        if (currentTarget != null)
        {
            // Point the light at the target
            transform.LookAt(currentTarget);
        }
    }

    // --- Called by UI Switch ---

    // Look at the big ball together
    public void LookAtSharedTarget()
    {
        currentTarget = sharedTarget;
    }

    // Look at its own small ball
    public void LookAtPersonalTarget()
    {
        if (personalTarget != null)
        {
            currentTarget = personalTarget;
        }
    }
}