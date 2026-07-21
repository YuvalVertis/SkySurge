using UnityEngine;

public sealed class Activation : MonoBehaviour
{
    [Header("Active Objects")]
    [SerializeField] GameObject[] activeObjects;

    [Header("Inactive Objects")]
    [SerializeField] GameObject[] inactiveObjects;

    [Header("Behaviour")]
    [SerializeField] bool destroyObjects;
    [SerializeField] bool repeatActivation;
    bool activated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (!activated || repeatActivation))
        {
            ActivationCollision();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (!activated || repeatActivation))
        {
            ActivationCollision();
        }
    }

    void ActivationCollision()
    {
        activated = true;
        foreach (var obj in activeObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        if (destroyObjects)
        {
            for (int i = inactiveObjects.Length - 1; i >= 0; i--)
            {
                if (inactiveObjects[i] != null)
                {
                    Destroy(inactiveObjects[i]);
                    inactiveObjects[i] = null;
                }
            }
        }
        else
        {
            foreach (var obj in inactiveObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
