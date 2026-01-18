using UnityEngine;

public class IdleRandomizer : MonoBehaviour
{
    public Animator animator;

    [Header("Timing")]
    public float checkInterval = 10f;

    [Header("Idle Variants")]
    public int idleVariantsCount = 4;

    [Header("Chance")]
    [Range(1, 6)]
    public int triggerThreshold = 4; // >4 = срабатывает

    float timer;
    bool isMoving;

    void Update()
    {
        if (isMoving)
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;
            TryPlayIdle();
        }
    }

    void TryPlayIdle()
    {
        int roll = Random.Range(1, 7); // 1–6

        if (roll > triggerThreshold)
        {
            int idleIndex = Random.Range(1, idleVariantsCount + 1);
            animator.SetInteger("idleIndex", idleIndex);
            animator.SetTrigger("idleVariant");
        }
    }

    public void SetMoving(bool moving)
    {
        isMoving = moving;
    }
}
