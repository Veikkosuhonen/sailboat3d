using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private Bounds _bounds;
    [SerializeField] public float density;

    private void Awake()
    {
        _bounds = GetComponent<BoxCollider>().bounds;
    }

    private void OnTriggerStay(Collider other)
    {
        var rigidBody = other.attachedRigidbody;
        var floating = other.gameObject.GetComponent<Floating>();
        if (!rigidBody || !floating) return;
        
        foreach (var floatPoint in floating.floatPoints)
        {
            var pos = floatPoint.transform.position;
            if (!_bounds.Contains(pos)) continue;

            var depth = Mathf.Clamp(pos.y, -2f, 0f);
            Debug.Log(depth);

            var lift = -density * depth;
            
            rigidBody.AddForceAtPosition(Vector3.up * lift * Time.deltaTime, pos);
        }
    }
    
}
