using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : CharacterBehaviour
{
    Collider2D enemyCollider;
    List<Transform> playersInRange = new List<Transform>();
    Transform nearestPlayer;
    Vector2 originalPosition;
    Animator animator;
    SpriteRenderer spriteRenderer;
    EnemyState currentState = EnemyState.Idle;
    
    [SerializeField] float speed = 1.5f;
    [SerializeField] float strength = 1f;

    [Networked]
    public float BaseHP { get; set; }

    [Networked]
    [HideInInspector]
    public float CurrentHP { get; set; }


    Image healthSlider;

    public override void Spawned()
    {
        enemyCollider = GetComponent<Collider2D>();
        originalPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        CurrentHP = BaseHP;
        healthSlider = transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>();
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    MovingTo(originalPosition);
                    break;

                case EnemyState.Chase:
                    MovingTo(nearestPlayer.position);
                    break;

                case EnemyState.Attack:
                    break;
            }
        }
    }

    public void PlayerEnterRange(Transform player)
    {
        playersInRange.Add(player);

        currentState = EnemyState.Chase;
        animator.SetFloat("MoveState", 1);

        FindNewPlayer();
    }
    public override void Render()
    {
        healthSlider.fillAmount = CurrentHP / BaseHP;
    }

    private void FindNewPlayer()
    {
        float nearestPlayerDistance = Mathf.Infinity;
        foreach (Transform playerTransform in playersInRange)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance < nearestPlayerDistance)
            {
                nearestPlayer = playerTransform;
                nearestPlayerDistance = distance;
            }
        }
    }

    public void PlayerOutOfRange(Transform player)
    {
        playersInRange.Remove(player);

        FindNewPlayer();

        if (playersInRange.Count == 0)
        {
            currentState = EnemyState.Idle;
            nearestPlayer = null;
        }
    }

    void MovingTo(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Runner.DeltaTime);

        if (transform.position.x - targetPosition.x > 0)
            FacingDirection = -1;
        else
            FacingDirection = 1;

        if (transform.position == (Vector3)targetPosition && currentState != EnemyState.Chase)
            animator.SetFloat("MoveState", 0);
    }

    protected override void ChangeDirection()
    {
        spriteRenderer.flipX = (FacingDirection < 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerProperties>().TakeDamageRpc(strength);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void TakeDamageRpc(float damage)
    {
        CurrentHP -= damage;

        healthSlider.fillAmount = CurrentHP / BaseHP;

        if (CurrentHP <= 0)
        {
            Runner.Despawn(Object);
        }
    }
}

public enum EnemyState
{
    Idle,
    Chase,
    Attack
}
