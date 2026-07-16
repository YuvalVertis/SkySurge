using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float maxSpeed;
    public float jumpSpeed;

    [SerializeField] float acceleration = 30f;
    [SerializeField] float checkDistance;
    [SerializeField] float gravityScaleMult;
    [SerializeField] Transform leftRay;
    [SerializeField] Transform midRay;
    [SerializeField] Transform rightRay;
    [SerializeField] LayerMask checkLayer;

    bool rotated, jumpPreesed, isGrounded, gravityModified, run;
    float moveX, defaultGravity;
    public bool fastFall = true;
    MovePlatform movePlatfrom;
    Rigidbody2D rb;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        if (GameObject.FindGameObjectsWithTag("Cloud").Length > 0)
        {
            movePlatfrom = GameObject.FindWithTag("Cloud").GetComponent<MovePlatform>();
        }

        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        isGrounded = Check();
        moveX = Input.GetAxisRaw("Horizontal");
        if (moveX != 0 && !run)
        {
            anim.SetBool("IsRunning", true);
            run = true;
        }
        else if (moveX == 0 && run)
        {
            anim.SetBool("IsRunning", false);
            run = false;
        }
    }

    public void OnJump(InputValue value)
    {
        if(value.isPressed && isGrounded)
        {
            jumpPreesed = true;
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
        if (movePlatfrom == null || !movePlatfrom.clouded)
        {
            rb.AddForce(Vector2.right * moveX * moveSpeed * acceleration, ForceMode2D.Force);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
        }
        if (movePlatfrom != null && movePlatfrom.clouded)
        {
            rb.velocity = new Vector2(moveSpeed * moveX + movePlatfrom.speed, rb.velocity.y);
        }
        if (jumpPreesed)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            jumpPreesed = false;
        }
        if (rb.velocity.y <= 0 && !isGrounded && !gravityModified)
        {
            rb.gravityScale *= gravityScaleMult; 
            gravityModified = true;
        }
        if(isGrounded && fastFall)
        {
            rb.gravityScale = defaultGravity;
            gravityModified = false;
        }
        if (moveX > 0 && rotated)
        {
            transform.eulerAngles = new Vector2(0, 0);
            rotated = false;
        }
        if (moveX < 0 && !rotated)
        {
            transform.eulerAngles = new Vector2(0, 180);
            rotated = true;
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
        if (!collision.gameObject.CompareTag("Finish")) return;
        HandleLevelCompletion();
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
}
