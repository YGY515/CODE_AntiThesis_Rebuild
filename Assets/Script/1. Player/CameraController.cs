using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset; 

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not set for PlayerCamera.");
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}