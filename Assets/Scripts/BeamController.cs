using UnityEngine;

class BeamController : MonoBehaviour {
    public float ropeLength;
    public float minLength = 0.1f;
    private SpringJoint springJoint;

    private void Awake() {
        springJoint = GetComponent<SpringJoint>();
    }

    private void FixedUpdate() {
        springJoint.maxDistance = ropeLength + minLength;
    }
}
