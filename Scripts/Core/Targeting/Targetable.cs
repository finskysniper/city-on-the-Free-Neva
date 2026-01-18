using UnityEngine;

public class Targetable : MonoBehaviour
{
    public int maxHp = 100;
    int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(DamageData damage)
    {
        currentHp -= damage.finalDamage;

        Debug.Log($"{name} получил {damage.finalDamage} урона");

        if (currentHp <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
