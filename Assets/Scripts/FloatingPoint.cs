using System;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPoint : MonoBehaviour
{
    public bool createMirror;

    private void Awake() {
        // Careful to not cause infinite chain!
        if (!createMirror || !transform.parent) return;

        var localPos = transform.localPosition;

        var newPos = Vector3.Reflect(localPos, transform.parent.right) + transform.parent.position;

        // Careful to not cause infinite chain!
        var newFp = Instantiate(this, newPos, transform.rotation);
        newFp.transform.parent = transform.parent;
        newFp.createMirror = false;
    }
}
