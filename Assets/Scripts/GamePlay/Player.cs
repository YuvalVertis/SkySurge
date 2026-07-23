using System.Collections;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [Header("Horiznotal Movement")]
    public float moveSpeed;
    private bool onPlatform;
    private MovePlatform activePlatform;

    [Header("Jump Logic")]
    public float jumpSpeed;
    public float gravityScaleMult;
    public bool fastFall = true;
    public float jumpBufferTimer = 0.1f;
    float jumpBufferCounter;


    [Header("Ground Check")]
    [SerializeField] float rayDistance;
    [SerializeField] Transform leftRay;
    [SerializeField] Transform midRay;
    [SerializeField] Transform rightRay;
    [SerializeField] LayerMask groundLayer;

    [Header("Other")]
    public InputReader inputHandler;
    public Vector2 blinkWaitRange;
    public bool allowJumpAnim;

    bool isGrounded, run;
    float direction, defaultGravity;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(BlinkingRoutine());
        defaultGravity = rb.gravityScale;
    }

    void OnEnable()
    {
        if (inputHandler == null) return;
        inputHandler.OnJump += PlayerJump;
        inputHandler.OnMove += PlayerMove;
    }

    void PlayerJump()
    {
        jumpBufferCounter = jumpBufferTimer;
    }

    void PlayerMove(float value)
    {
        direction = value;
        if (value > 0)
        {
            sprite.flipX = false;
        }
        else if (value < 0)
        {
            sprite.flipX = true;
        }

        bool isMoving = value != 0;
        if (isMoving != run)
        {
            anim.SetBool(CodesManager.Run, isMoving);
            run = isMoving;
        }
    }

    bool Check()
    {
        if (leftRay == null || midRay == null || rightRay == null) return false;

        return Physics2D.Raycast(leftRay.position, Vector2.down, rayDistance, groundLayer)
            || Physics2D.Raycast(midRay.position, Vector2.down, rayDistance, groundLayer)
            || Physics2D.Raycast(rightRay.position, Vector2.down, rayDistance, groundLayer);
    }

    void Update()
    {
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        if (!allowJumpAnim) return;
        anim.SetBool("JumpAnim", allowJumpAnim);
        anim.SetBool(CodesManager.IsGrounded, isGrounded);
        anim.SetFloat(CodesManager.JumpVel, rb.velocity.y);
    }

    void FixedUpdate()
    {
        //Ground check with 3 raycasts
        isGrounded = Check();

        float targetSpeedX = moveSpeed * direction;
        float targetSpeedY = rb.velocity.y;

        if (onPlatform && activePlatform != null)
        {
            targetSpeedX += activePlatform.velocity.x;

            if (activePlatform.velocity.y < 0f && isGrounded)
            {
                targetSpeedY = activePlatform.velocity.y;
            }
        }

        rb.velocity = new Vector2(targetSpeedX, targetSpeedY);

        if (jumpBufferCounter > 0f && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpBufferCounter = 0f;
        }

        //Fall
        FallLogic();
    }

    void FallLogic()
    {
        if (!fastFall) return;

        if (rb.velocity.y <= 0 && !isGrounded)
        {
            rb.gravityScale = defaultGravity * gravityScaleMult;
        }
        if (isGrounded)
        {
            rb.gravityScale = defaultGravity;
        }
    }

    void HandleLevelCompletion()
    {
        ScenesHandler.NextLevel();
        ScenesHandler.LoadSceneByIndex(Levels.Levels);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ScenesHandler.LoadSceneByIndex(Levels.Levels);
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            HandleLevelCompletion();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<MovePlatform>(out MovePlatform platform))
        {
            activePlatform = platform;
            onPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<MovePlatform>(out MovePlatform platform))
        {
            if (activePlatform == platform)
            {
                activePlatform = null;
                onPlatform = false;
            }
        }
    }

    IEnumerator BlinkingRoutine()
    {
        while (true)
        {
            while (direction != 0 || !isGrounded)
            {
                yield return null;
            }

            float timer = Random.Range(blinkWaitRange.x, blinkWaitRange.y);
            while (timer > 0)
            {
                if (direction != 0 || !isGrounded) break;

                timer -= Time.deltaTime;
                yield return null;
            }

            if (direction == 0 && isGrounded)
            {
                anim.SetTrigger(CodesManager.Blink);
            }
            yield return null;
        }
    }

    void OnDisable()
    {
        if (inputHandler == null) return;

        inputHandler.OnJump -= PlayerJump;
        inputHandler.OnMove -= PlayerMove;
    }
}
