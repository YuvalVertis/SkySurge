using UnityEngine;

public sealed class Chest : MonoBehaviour
{
    [SerializeField] BoxCollider2D playerC;
    BoxCollider2D chestC;
    Animator anim;
    bool open;

    void Awake()
    {
        chestC = gameObject.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(chestC, playerC, true);
        anim= GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !open)
        {
            anim.SetTrigger("Open");
            open = true;
        }
    }
}
