using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpSpeed;
    public float gravityScaleMult;
    public bool fastFall = true;
    public InputReader inputHandler;

    [Header("Ground Check")]
    [SerializeField] float rayDistance;
    [SerializeField] Transform leftRay;
    [SerializeField] Transform midRay;
    [SerializeField] Transform rightRay;
    [SerializeField] LayerMask groundLayer;

    [Header("Other")]
    public Vector2 blinkWaitRange;
    public bool allowJumpAnim;

    bool jumpPressed, isGrounded, run;
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
        inputHandler.OnJump += PlayerJump;
        inputHandler.OnMove += PlayerMove;
    }

    void PlayerJump()
    {
        if (isGrounded)
        {
            jumpPressed = true;
        }
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
        return Physics2D.Raycast(leftRay.position, Vector2.down, rayDistance, groundLayer)
            || Physics2D.Raycast(midRay.position, Vector2.down, rayDistance, groundLayer)
            || Physics2D.Raycast(rightRay.position, Vector2.down, rayDistance, groundLayer);
    }

    void Update()
    {
        if (!allowJumpAnim) return;
        anim.SetBool(CodesManager.IsGrounded, isGrounded);
        anim.SetFloat(CodesManager.JumpVel, rb.velocity.y);
    }

    void FixedUpdate()
    {
        //Ground check with 3 raycasts
        isGrounded = Check();

        rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);

        if (jumpPressed && isGrounded)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            jumpPressed = false;
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
        int currentLevel = SceneManager.GetActiveScene().buildIndex - 1;
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevel == unlockedLevel && unlockedLevel < 8)
        {
            unlockedLevel++;
            PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
            PlayerPrefs.Save();
        }

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

    IEnumerator BlinkingRoutine()
    {
        while (true)
        {
            while (direction != 0)
            {
                yield return null;
            }

            float timer = Random.Range(blinkWaitRange.x, blinkWaitRange.y);
            while (timer > 0)
            {
                if (direction != 0) break;

                timer -= Time.deltaTime;
                yield return null;
            }

            if (direction == 0)
            {
                anim.SetTrigger(CodesManager.Blink);
            }
        }
    }
}
