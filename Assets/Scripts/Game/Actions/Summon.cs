using System.Collections;
using UnityEngine;

public sealed class Summon : MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] float destroyTime;
    [SerializeField] GameObject spawnObject;
    float startTime;
    bool tracker;

    void Awake()
    {
        cooldown = Random.Range(2.5f, 3.5f);
        startTime = cooldown;
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
        Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
        GameObject newObject = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
        if (newObject != null)
        {
            Destroy(newObject, destroyTime);
        }
        tracker = false;
        cooldown = startTime;
    }
}
