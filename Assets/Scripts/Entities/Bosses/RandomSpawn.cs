using System.Collections;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] GameObject spawnObject;
    BoxCollider2D boxCollider;
    Health health;
    float startTime;
    bool tracker;
    private void Awake()
    {
        startTime = cooldown;
        boxCollider  = gameObject.GetComponent<BoxCollider2D>();
        health = GameObject.Find("EnemyCloud").GetComponent<Health>();
    }
    private void Update()
    {

        if (cooldown > 0 && health.currentHealth <= 3)
        {
            cooldown -= Time.deltaTime;
        }
        if (cooldown <= 0 && health.currentHealth <= 3 && !tracker)
        {
            InvokeRepeating("SpawnWithDelay", 3f, 3f); 
            tracker = true;
        }
    }
    IEnumerator SpawnObject()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x), transform.position.y);
        GameObject newObject = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
        if (newObject != null)
        {
            Destroy(newObject, 2.5f);
        }
        yield return null;
    }
    void SpawnWithDelay()
    {
        StartCoroutine(SpawnObject());
        cooldown = startTime;
    }
}
