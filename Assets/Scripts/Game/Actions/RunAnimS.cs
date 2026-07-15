using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAnimS : MonoBehaviour
{
    bool tracker;
    [SerializeField] Animator anim;
    [SerializeField] float speed;
    private void Awake()
    {
        if(!tracker)
        {
            anim.SetFloat("Speed", speed);
        }
    }
}
