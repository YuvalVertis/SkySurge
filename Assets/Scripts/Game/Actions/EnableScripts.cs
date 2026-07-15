using UnityEngine;

public class EnableScripts : MonoBehaviour
{
    CameraX mainCamera;

    void Awake()
    {
        mainCamera = GameObject.Find("Camera").GetComponent<CameraX>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {   
        if(collision.gameObject.CompareTag("Player") && mainCamera.enabled == false)
        {
            Animator playerAnimator = collision.gameObject.GetComponent<Animator>();
            playerAnimator.SetFloat("Speed", 1.2f);
            mainCamera.enabled = true;
        }
    }
}
