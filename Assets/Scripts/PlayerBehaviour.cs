using Cinemachine;
using Fusion;
using UnityEngine;

public class PlayerBehaviour : CharacterBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    PlayerProperties properties;
    CinemachineVirtualCamera virtualCamera;
    GameObject chatPanel;

    bool attackInput;
    bool isMoving = true;
    bool toggleChatPanel = false;
    
    [HideInInspector] public bool isAttacking;
    [SerializeField] BoxCollider2D attackCollider;

    public static InputType input = InputType.Playing;

    public override void Spawned()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        properties = GetComponent<PlayerProperties>();

        if (HasStateAuthority)
        {
            virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
            virtualCamera.Follow = transform;
        }

        chatPanel = GameObject.Find("Chat Panel");

        if(chatPanel != null)
            chatPanel.SetActive(false);
    }

    public void Update()
    {
        if (input != InputType.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ToggleChat();
            }

            return;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleChat();
        }   

        if (Input.GetMouseButtonDown(0))
        {
            attackInput = true;
            isMoving = false;
        }
    }

    private void ToggleChat()
    {
        toggleChatPanel = !toggleChatPanel;
        chatPanel.SetActive(toggleChatPanel);
        input = (toggleChatPanel) ? InputType.Chat : InputType.Playing;
    }

    public override void FixedUpdateNetwork()
    {
        if(input != InputType.Playing)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if (HasStateAuthority)
        {
            if (isMoving)
            {
                Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

                if (moveVector.x != 0)
                    FacingDirection = moveVector.x;

                if (moveVector != Vector3.zero)
                    animator.SetFloat("MoveState", 1);
                else
                    animator.SetFloat("MoveState", 0);

                rb.velocity = moveVector * properties.MoveSpeed * Runner.DeltaTime;
            }

            if (attackInput && !isAttacking)
            {
                rb.velocity = Vector3.zero;
                attackInput = false;
                isAttacking = true;
                RpcAttackTrigger();
            }
        }

        Debug.Log(Object.HasInputAuthority);
        Debug.Log(Object.HasStateAuthority);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcAttackTrigger()
    {
        if (!HasInputAuthority)
            return;

        AudioManager.Instance.PlayAudioRpc("Sword", "Master/SFX", transform.position);
        animator.SetTrigger("Attack");
    }

    public void AttackDone()
    {
        isAttacking = false;
        isMoving = true;
    }

    public void ToggleAttackCollider(int value)
    {
        attackCollider.enabled = (value == 0);
        attackCollider.gameObject.transform.localScale = 
            (spriteRenderer.flipX) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    protected override void ChangeDirection()
    {
        spriteRenderer.flipX = (FacingDirection < 0);
    }
}

public enum InputType
{
    Playing,
    Chat
}
