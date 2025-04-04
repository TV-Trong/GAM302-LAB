using UnityEngine;

public class EnemyDetectPlayer : MonoBehaviour
{
    EnemyBehaviour enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.PlayerEnterRange(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.PlayerOutOfRange(collision.transform);
        }
    }
}
