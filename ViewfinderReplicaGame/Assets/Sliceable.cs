using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Sliceable : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(transform.gameObject.GetComponent<Collider>().ClosestPointOnBounds(other.transform.position));
    }

    struct Triangle
    {
        Vector3 point, normal, uv;
    }
}
