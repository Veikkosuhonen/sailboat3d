using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBody : MonoBehaviour
{
    public new Rigidbody rigidbody;

    [SerializeField] private Vector3 hullDrag;

    public bool inWater;

    private void FixedUpdate() {
        if (!inWater) return;

        var velocity = rigidbody.velocity;

        var dragRight = Vector3.Dot(velocity, transform.right);
        var dragUp = Vector3.Dot(velocity, transform.up);
        var dragForward = Vector3.Dot(velocity, transform.forward);

        var goingBackward = Mathf.Sign(dragRight) * 0.5f + 0.5f;

        var relativeDragForce = -new Vector3(
            dragRight * hullDrag.x + 0.5f * hullDrag.y * goingBackward,
            dragUp * hullDrag.y,
            dragForward * hullDrag.z
        );

        var worldSpaceForce = transform.localToWorldMatrix * relativeDragForce;

        rigidbody.AddForceAtPosition(worldSpaceForce, transform.position);
    }
}
