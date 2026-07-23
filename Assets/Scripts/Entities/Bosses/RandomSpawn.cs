using System.Collections;
using UnityEngine;

public sealed class RandomSpawn : MonoBehaviour
{
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] BoxCollider2D rangeBox;
    [SerializeField] Vector2Int spawnCountRange;
    [SerializeField] float spawnCooldown;
    [SerializeField] float startDelay;


    Coroutine spawnRoutine;
    public bool spawn = true;

    void Start()
    {
        spawnRoutine = StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        while (true)
        {
            yield return new WaitUntil(() => spawn);

            if(startDelay > 0)
            {
                yield return new WaitForSeconds(startDelay);
                startDelay = 0;
            }

            int count = Random.Range(spawnCountRange.x, spawnCountRange.y + 1);

            for (int i = 0; i < count; i++)
            {
                Vector2 spawnPosition = new Vector2(Random.Range(rangeBox.bounds.min.x, rangeBox.bounds.max.x),transform.position.y);
                GameObject newObject = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
                Destroy(newObject, 2.5f);

                if(count > 1)
                {
                    yield return new WaitForSeconds(0.25f);
                }
            }

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


