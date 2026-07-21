using System.Collections;
using UnityEngine;

public sealed class RandomSpawn : MonoBehaviour
{
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] BoxCollider2D rangeBox;
    [SerializeField] float spawnCooldown;
    Coroutine spawnRoutine;
    [SerializeField] bool spawn = true;

    void Start()
    {
        spawnRoutine = StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        while (spawn)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(rangeBox.bounds.min.x, rangeBox.bounds.max.x), transform.position.y);

            GameObject newObject = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
            Destroy(newObject, 2.5f);

            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
    }
}


