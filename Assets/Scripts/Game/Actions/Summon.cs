using System.Collections;
using UnityEngine;

public sealed class Summon : MonoBehaviour
{
    [SerializeField] float destroyTime;
    [SerializeField] GameObject spawnObject;
    Coroutine spawnRoutine;

    void Start()
    {
        spawnRoutine = StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2.5f, 3.5f));

            Vector2 spawnPosition = transform.position;
            GameObject newObject = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
            Destroy(newObject, destroyTime);
        }
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
    }
}
