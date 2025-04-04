using Fusion;
using UnityEngine;

public class PlayerAttackCollider : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (Object.HasInputAuthority)
                collision.GetComponent<EnemyBehaviour>().TakeDamageRpc(4);
        }
    }
}
