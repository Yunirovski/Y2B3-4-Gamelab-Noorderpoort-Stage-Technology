using UnityEngine;

public class LightLookAt : MonoBehaviour
{
    [Header("Target To Focus On")]
    public Transform target;

    void LateUpdate()
    {
        if (target != null)
        {
            // Keep the light's Z-axis pointing straight at the target
            transform.LookAt(target);
        }
    }
}