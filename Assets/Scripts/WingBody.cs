using System.Collections;
using UnityEngine;
public class WingBody : MonoBehaviour {

    public bool log;
    public float dragCoeff;
    public AnimationCurve liftCoeff;
    public float wingArea;
    public Vector3 wind;
    public FluidType fluidType = FluidType.Air;
    private float fluidDensity;
    static float AIR_DENSITY = 0.01f;
    static float WATER_DENSITY = 1f;

    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        fluidDensity = fluidType == FluidType.Air ? AIR_DENSITY : WATER_DENSITY;
    }

    private void FixedUpdate() {
        var force = getWingLiftDragForce(transform.right, wind - rigidbody.velocity, wingArea, dragCoeff);
        rigidbody.AddForce(force);
    }

    private Vector3 getWingLiftDragForce(Vector3 wingDirection, Vector3 flow, float wingArea, float dragCoeff) {
        // Assume that windDirection defines the orientation of a plane (the wing). Therefore it should be the 'right' or 'left' vector of the wing-object's transform.

        Vector3 flowDirection = flow.normalized;
        float flowVelocity2 = flow.sqrMagnitude;

        // Angle of attack is 1 when the plane is perpendicular to flow (flow is its normal and the angle is zero) and 0 when the wing would have no visible area from the flow's direction.
        float wingAngleOfAttack = Mathf.Cos(Vector3.Angle(wingDirection, flowDirection) * Mathf.Deg2Rad);
        float drag = wingAngleOfAttack * wingAngleOfAttack * dragCoeff * wingArea * flowVelocity2 * fluidDensity;
        Vector3 dragForce = flowDirection * drag;

        float lift = Mathf.Sign(wingAngleOfAttack) * liftCoeff.Evaluate(Mathf.Abs(wingAngleOfAttack));
        if (log) Debug.Log(lift);

        lift *= wingArea * flowVelocity2 * fluidDensity;

        // Mentally visualise this by imagining your hand out of the car window.
        //Vector3 
        Vector3 liftDirection = Quaternion.AngleAxis(90, flowDirection) * Vector3.Cross(wingDirection, flowDirection);

        Vector3 liftForce = liftDirection * lift;

        var totalForce = liftForce + dragForce;

        Debug.DrawLine(transform.position, transform.position + liftForce, Color.blue);
        Debug.DrawLine(transform.position, transform.position + dragForce, Color.red);
        Debug.DrawLine(transform.position, transform.position + totalForce, Color.yellow);

        return totalForce;
    }
}