using UnityEngine;

public class EnemyStatistics : MonoBehaviour
{
    public int Health;
    public bool IsTrigered;
    public bool IsSleep;

    public void GetHit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
