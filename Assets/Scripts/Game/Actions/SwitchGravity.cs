using UnityEngine;

public sealed class SwitchGravity : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float mult;
    public CameraY cameraY;
    bool done;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !done)
        {
            rb.gravityScale *= mult;
            cameraY.offset.y = 3.25f;
            done = true;
        }
    }
}
