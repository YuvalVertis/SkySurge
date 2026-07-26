using UnityEngine;

public sealed class GoDown : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float newGravity;
    [SerializeField] CameraY cameraY;
    [SerializeField] CameraX cameraX;
    [SerializeField] GameObject border;
    Player player;
    bool done;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || done) return;
        if (rb == null || cameraX == null || cameraY == null) return;

        player = collision.gameObject.GetComponent<Player>();
        player.defaultGravity = newGravity;

        rb.gravityScale = newGravity;
        cameraY.offset.y = 3.25f;

        Invoke(nameof(EnableScripts), 1f);
        done = true;
    }

    void EnableScripts()
    {
        if (player != null)
        {
            player.fastFall = true;
        }
        if (!cameraX.enabled)
        {
            cameraX.enabled = true;
        }
        if(border != null)
        {
            border.SetActive(false);
        }
    }
}
