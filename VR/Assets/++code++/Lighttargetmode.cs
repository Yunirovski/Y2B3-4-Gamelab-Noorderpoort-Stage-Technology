using UnityEngine;

public class Lighttargetmode : MonoBehaviour
{
    [Header("All Lights")]
    // Put all your stage lights here
    public LightLookAt[] allStageLights;

    [Header("Big Ball vs Small Balls")]
    // The big ball for focus mode
    public MeshRenderer sharedTargetMesh;
    // The small balls for scattered mode
    public MeshRenderer[] personalTargetMeshes;

    // Connect this to your UI Toggle button
    public void ToggleLightMode(bool isScattered)
    {
        // 1. Change where the lights look
        foreach (var light in allStageLights)
        {
            if (isScattered) light.LookAtPersonalTarget();
            else light.LookAtSharedTarget();
        }

        // 2. Hide or show the big ball
        if (sharedTargetMesh != null)
        {
            sharedTargetMesh.enabled = !isScattered;
        }

        // 3. Hide or show the small balls
        foreach (var mesh in personalTargetMeshes)
        {
            if (mesh != null)
            {
                mesh.enabled = isScattered;
            }
        }
    }
}