using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private Transform target;

    private void Start()
    {
        target = GameObject.FindWithTag("MainCamera").transform;
    }

    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
