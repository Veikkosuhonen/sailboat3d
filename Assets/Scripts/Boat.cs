using UnityEngine;

public class Boat : MonoBehaviour {
    private ConfigurableJoint rudderJoint;
    private float targetAngle;

    private void Awake() {
        rudderJoint = GetComponent<ConfigurableJoint>();
    }

    public void Steer(float delta) {
        targetAngle += delta;
        targetAngle = Mathf.Clamp(targetAngle, -50f, 50f);
        Debug.Log("Steering " + targetAngle);
        rudderJoint.targetRotation = Quaternion.Euler(targetAngle, 0f, 0f);
    }

}
