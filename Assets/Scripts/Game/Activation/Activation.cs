using UnityEngine;

public sealed class Activation : MonoBehaviour
{
    [Header("Active Objects")]
    [SerializeField] GameObject[] aObjects;

    [Header("Deactive Objects")]
    [SerializeField] GameObject[] dObjects;
    [SerializeField] int enableRange;
    [SerializeField] int disableRange;
    [SerializeField] bool destroy;
    bool tracker, tracker2;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        HandleCollisionWithPlayer();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        HandleCollisionWithPlayer();
    }

    void HandleCollisionWithPlayer()
    {
        if (aObjects.Length == 1 && !aObjects[0].activeSelf)
        {
            aObjects[0].SetActive(true);
        }
        else if(aObjects.Length > 1)
        {
            for (int i = 0; i < enableRange && !tracker; i++)
            {
                aObjects[i].SetActive(true); 
            }
            tracker = true;
        }
        if (dObjects.Length == 1)
        {
            if (!destroy && dObjects[0].activeSelf)
            {
                dObjects[0].SetActive(false); 
            }
            else if (destroy && dObjects[0].activeSelf)
            {
                Destroy(dObjects[0]);
            }
        }
        if(dObjects.Length > 1)
        {
            for (int i = 0; i < disableRange && !tracker2; i++)
            {
                if (!destroy)
                {
                    dObjects[i].SetActive(false);
                }
                else
                {
                    Destroy(dObjects[i]);
                }
            }
            tracker2 = true;
        }
    }
}
