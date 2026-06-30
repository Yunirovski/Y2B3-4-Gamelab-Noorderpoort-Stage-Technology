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

    [Header("Read Only")]
    public bool isStarInRange = false;

    private float fractionalScore = 0f;

    void LateUpdate()
    {
        if (controlledTarget != null)
        {
            transform.LookAt(controlledTarget);
        }

        isStarInRange = false;

        if (controlledTarget != null && scoringTarget != null && scoreManager != null)
        {
            float distance = Vector3.Distance(controlledTarget.position, scoringTarget.position);

            AudienceMeter meter = Object.FindAnyObjectByType<AudienceMeter>();

            isStarInRange = distance <= scoreDistance;

            if (isStarInRange && (meter == null || meter.emotionValue < 100f))
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