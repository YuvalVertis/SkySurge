using UnityEngine.SceneManagement;
using UnityEngine;

public sealed class Player : MonoBehaviour
{

    [Header("Movement")]
    public float acceleration;
    public float moveSpeed;
    public float maxSpeed;
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

    bool jumpPressed, isGrounded, run;
    float direction, defaultGravity;
    MovePlatform movePlatfrom;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        if (GameObject.FindGameObjectsWithTag("Cloud").Length > 0)
        {
            movePlatfrom = GameObject.FindWithTag("Cloud").GetComponent<MovePlatform>();
        }

        defaultGravity = rb.gravityScale;
    }

    void OnEnable()
    {
        inputHandler.OnJump += PlayerJump;
        inputHandler.OnMove += PlayerMove;
    }

    void PlayerJump()
    {
        if(isGrounded)
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

    void FixedUpdate()
    {
        //Ground check with 3 raycasts
        isGrounded = Check();

        //Normal movement
        if (movePlatfrom == null || !movePlatfrom.clouded)
        {
            rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
        }

        // OnPlatform
        if (movePlatfrom != null && movePlatfrom.clouded)
        {
            rb.velocity = new Vector2(moveSpeed * direction + movePlatfrom.speed, rb.velocity.y);
        }

        if (jumpPressed && isGrounded)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            jumpPressed = false;
        }

        //Fall
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
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (unlockedLevel == 8)
        {
            unlockedLevel = 1;
        }
        else
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex - 1;
            if (currentLevel == unlockedLevel)
            {
                unlockedLevel++;
            }
        }
        PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
        PlayerPrefs.Save();
        ScenesHandler.LoadSceneByIndex(Levels.Levels);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            HandleLevelCompletion();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ScenesHandler.LoadSceneByIndex(Levels.Levels);
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            HandleLevelCompletion();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(leftRay.position, Vector2.down * rayDistance);
        Gizmos.DrawRay(midRay.position, Vector2.down * rayDistance);
        Gizmos.DrawRay(rightRay.position, Vector2.down * rayDistance);
    }
}
