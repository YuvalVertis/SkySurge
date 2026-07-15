using System.Collections;
using UnityEngine;

public class GenRndSpawn : MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] float destroyTime;
    [SerializeField] GameObject spawnObject;
    BoxCollider2D boxCollider;
    float startTime;
    bool tracker;

    void Awake()
    {
        startTime = cooldown;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        if (cooldown <= 0 && !tracker)
        {
            StartCoroutine(SpawnObject());
            tracker = true;
        }
    }

    IEnumerator SpawnObject()
    {
        yield return null;
        Vector2 spawnPosition = new Vector2(Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x), transform.position.y);
        GameObject newObject = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
        if (newObject != null)
        {
            Destroy(newObject, destroyTime);
        }
        tracker = false;
        cooldown = startTime;
    }
}
