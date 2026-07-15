using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] BoxCollider2D playerC;
    BoxCollider2D chestC;
    Animator anim;
    bool open;
    private void Awake()
    {
        chestC = gameObject.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(chestC, playerC, true);
        anim= GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !open)
        {
            anim.SetTrigger("Open");
            open = true;
        }
    }
}
