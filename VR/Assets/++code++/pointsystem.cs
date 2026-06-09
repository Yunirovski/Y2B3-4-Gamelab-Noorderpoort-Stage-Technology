using UnityEngine;

public class pointsystem : MonoBehaviour
{
    [Header("Tracking Settings")]
    public Transform controlledTarget;
    public Transform scoringTarget;
    public float scoreDistance = 3.0f;

    [Header("Score Settings")]
    public GameScoreManager scoreManager;
    public float baseScorePerSecond = 10f;

    private float fractionalScore = 0f;

    void LateUpdate()
    {
        // Look at the target you control
        if (controlledTarget != null)
        {
            transform.LookAt(controlledTarget);
        }

        if (controlledTarget != null && scoringTarget != null && scoreManager != null)
        {
            float distance = Vector3.Distance(controlledTarget.position, scoringTarget.position);

            
            AudienceMeter meter = Object.FindAnyObjectByType<AudienceMeter>();

            if (distance <= scoreDistance && (meter == null || meter.emotionValue < 100f))
            {
                int multiplier = scoreManager.currentBonus;

                
                fractionalScore += baseScorePerSecond * Time.deltaTime * multiplier;

                if (fractionalScore >= 1f)
                {
                    int pointsToAdd = Mathf.FloorToInt(fractionalScore);
                    scoreManager.currentScore += pointsToAdd;
                    fractionalScore -= pointsToAdd;
                }
            }
        }
    }
}