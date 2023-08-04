using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    private HashSet<Collider> contacts = new();
    private bool inWater = false;
    private int rbCount = 0;
    public PlayerController player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("water")) inWater = true;
        else {
            contacts.Add(other);
            if (other.attachedRigidbody) rbCount++;
        }

        if (other.transform.parent.CompareTag("boat")) {
            player.boat = other.transform.parent.GetComponent<Boat>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("water")) inWater = false;
        else {
            contacts.Remove(other);
            if (other.attachedRigidbody) rbCount--;
        }

        if (other.transform.parent.CompareTag("boat")) {
            player.boat = null;
        }
    }

    public bool AddForce(Vector3 force) {
        if (contacts.Count == 0) {
            if (inWater) {
                return true;
            }
            return false;
        }

        var individualForce = force / rbCount;
        foreach (var go in contacts) {
            var rb = go.GetComponent<Rigidbody>();
            if (!rb) rb = go.GetComponentInParent<Rigidbody>(); // Boat works like this :(

            if (rb) {
                rb.AddForceAtPosition(individualForce, go.ClosestPoint(transform.position));
            }
            
        }
        return true;
    }

    public Vector3 GetRelativeVelocity(Rigidbody body) {
        if (rbCount == 0) {
            return body.velocity;
        }

        var averageVelocity = new Vector3();

        foreach (var go in contacts) {
            var rb = go.GetComponent<Rigidbody>();
            if (rb)
                averageVelocity += rb.velocity;
        }

        averageVelocity /= rbCount;

        return body.velocity - averageVelocity;
    }

}
