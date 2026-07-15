using UnityEngine.SceneManagement;
using UnityEngine;
public sealed class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    MovePlatform movePlatfrom;
    public float moveSpeed, maxSpeed;
    public float jumpSpeed;
    [SerializeField] float acceleration = 30f, checkRadius, gravityScaleMult;
    [SerializeField] LayerMask checkLayer;
    Transform checkPos;
    float moveX, defaultGravity;
    bool rotated, jumped, grounded, gravityModified, run;
    public bool fastFall = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        checkPos = transform.Find("CheckPos");

        if (GameObject.FindGameObjectsWithTag("Cloud").Length > 0)
        {
            movePlatfrom = GameObject.FindWithTag("Cloud").GetComponent<MovePlatform>();
        }

        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jumped = true;
        }
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

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(checkPos.position, checkRadius, checkLayer);
        if (movePlatfrom == null || !movePlatfrom.clouded)
        {
            rb.AddForce(Vector2.right * moveX * moveSpeed * acceleration, ForceMode2D.Force);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
        }
        if (movePlatfrom != null && movePlatfrom.clouded)
        {
            rb.velocity = new Vector2(moveSpeed * moveX + movePlatfrom.speed, rb.velocity.y);
        }
        if (jumped)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            jumped = false;
        }
        if (rb.velocity.y <= 0 && !grounded && !gravityModified)
        {
            rb.gravityScale *= gravityScaleMult; 
            gravityModified = true;
        }
        if(grounded && fastFall)
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
