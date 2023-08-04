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
        var floating = other.gameObject.GetComponentInParent<Floating>();
        var rigidBody = other.gameObject.GetComponentInParent<Rigidbody>();

        if (!rigidBody || !floating) return;
        
        foreach (var floatPoint in floating.floatPoints)
        {
            var pos = floatPoint.gameObject.transform.position;
            if (!_bounds.Contains(pos)) continue;
        
            var depth = Mathf.Clamp(pos.y, -2f, 0f);

            var lift = -density * depth;
            
            rigidBody.AddForceAtPosition(Vector3.up * lift * Time.deltaTime, pos);
        }
    }

    private void OnTriggerEnter(Collider other) {
        var bb = other.gameObject.GetComponent<BoatBody>();
        if (!bb) return;
        bb.inWater = true;
    }

    private void OnTriggerExit(Collider other) {
        var bb = other.gameObject.GetComponent<BoatBody>();
        if (!bb) return;
        bb.inWater = false;
    }



}
