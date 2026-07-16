using System.Collections;
using UnityEngine;

public sealed class RandomSpawn : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] float cooldown;
    BoxCollider2D rangeBox;
    Coroutine spawnRoutine;

    void Awake()
    {
        rangeBox = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        spawnRoutine = StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        while (true)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(rangeBox.bounds.min.x, rangeBox.bounds.max.x), transform.position.y);

            GameObject newObject = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
            Destroy(newObject, 2.5f);

            yield return new WaitForSeconds(cooldown);
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


