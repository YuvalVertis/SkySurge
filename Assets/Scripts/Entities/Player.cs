using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;

public sealed class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float maxSpeed;
    public float jumpSpeed;

    [SerializeField] float acceleration = 30f;
    [SerializeField] float checkDistance;
    [SerializeField] float gravityScaleMult;

    [Header("Ground Check")]
    [SerializeField] Transform leftRay;
    [SerializeField] Transform midRay;
    [SerializeField] Transform rightRay;
    [SerializeField] LayerMask checkLayer;
    public bool allowFastFall = true;
    bool jumpPressed, isGrounded, gravityModified, run;
    float moveX, defaultGravity;
    MovePlatform movePlatfrom;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;

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

    public void OnMove(InputValue value)
    {
        moveX = value.Get<float>();

        if (moveX > 0)
        {
            sprite.flipX = false;
        }
        else if (moveX < 0)
        {
            sprite.flipX = true;
        }

        bool isMoving = moveX != 0;
        if (isMoving != run)
        {
            anim.SetBool(CodesManager.Run, isMoving);
            run = isMoving;
        }
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            jumpPressed = true;
        }
    }

    bool Check()
    {
        return Physics2D.Raycast(leftRay.position, Vector2.down, checkDistance, checkLayer)
            || Physics2D.Raycast(midRay.position, Vector2.down, checkDistance, checkLayer)
            || Physics2D.Raycast(rightRay.position, Vector2.down, checkDistance, checkLayer);
    }

    void FixedUpdate()
    {
        isGrounded = Check();
        //Normal movement
        if (movePlatfrom == null || !movePlatfrom.clouded)
        {
            rb.AddForce(Vector2.right * moveX * moveSpeed * acceleration, ForceMode2D.Force);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
        }
        // OnPlatform
        if (movePlatfrom != null && movePlatfrom.clouded)
        {
            rb.velocity = new Vector2(moveSpeed * moveX + movePlatfrom.speed, rb.velocity.y);
        }
        if (jumpPressed)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            jumpPressed = false;
        }

        if (!allowFastFall) return;

        if (rb.velocity.y <= 0 && !isGrounded && !gravityModified)
        {
            rb.gravityScale *= gravityScaleMult;
            gravityModified = true;
        }
        if (isGrounded)
        {
            rb.gravityScale = defaultGravity;
            gravityModified = false;
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
        SceneManager.LoadScene("Levels");
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
            SceneManager.LoadScene("Levels");
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            HandleLevelCompletion();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(leftRay.position, Vector2.down * checkDistance);
        Gizmos.DrawRay(midRay.position, Vector2.down * checkDistance);
        Gizmos.DrawRay(rightRay.position, Vector2.down * checkDistance);
    }
}
