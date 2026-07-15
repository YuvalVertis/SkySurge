using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordActions : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject disableObject;
    [SerializeField] float actionCordY;
    bool tracker;
    private void Update()
    {
        if(target.position.y >= actionCordY && disableObject.activeSelf)
        {
            disableObject.SetActive(false);
        }
    }

}
