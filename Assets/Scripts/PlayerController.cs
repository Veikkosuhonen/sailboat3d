using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _body;
    private Collider collider;

    [SerializeField] public float jumpForce;
    [SerializeField] public float moveForce;
    [SerializeField] public float panSpeedX;
    [SerializeField] public float panSpeedY;
    [SerializeField] public float panMinY;
    [SerializeField] public float panMaxY;
    [SerializeField] public GameObject camTargetPosition;
    [SerializeField] public GameObject camTarget;
    [SerializeField] public GameObject feetObject;
    private Feet feet;

    private Vector3 _moveInputDirection;
    private float steerDirection;
    private Vector2 _currentPan;

    [SerializeField] public float smoothTime;
    private float _currentVelocity;

    public Boat boat;
    
    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        feet = feetObject.GetComponent<Feet>();
        feet.player = this;
    }

    private void Update()
    {
        MoveUpdate();
        CamDirUpdate();
    }

    private void FixedUpdate() {
        if (boat)
            boat.Steer(steerDirection);
    }

    private void CamDirUpdate()
    {
        camTarget.transform.rotation = Quaternion.Euler(_currentPan.y, _currentPan.x, 0f);
        camTarget.transform.position = camTargetPosition.transform.position;
    }

    private void MoveUpdate()
    {
        var moveVector = _moveInputDirection;

        if (moveVector.sqrMagnitude == 0) {
            collider.material.dynamicFriction = 1f;
            collider.material.staticFriction = 1f;
            return;
        }
        collider.material.dynamicFriction = 0f;
        collider.material.staticFriction = 0f;

        // target direction is relative to camTarget direction. Rotate it:
        var actualDirection = Quaternion.Euler(0f, camTarget.transform.eulerAngles.y, 0f) * moveVector;
        // Get the Y angle
        var targetAngle = Mathf.Atan2(actualDirection.x, actualDirection.z) * Mathf.Rad2Deg;
        // Smooth it
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        // Set the character angle
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        // Force direction. Not smoothed
        var moveForceVec = actualDirection * moveForce * Time.deltaTime;


        // Apply force if feet touching something
        if (feet.AddForce(-moveForceVec))
            _body.AddForce(moveForceVec);
    }

    public void Move(InputAction.CallbackContext context) 
    {
        var input = context.ReadValue<Vector2>();
        _moveInputDirection = new Vector3(input.x, 0f, input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump, " + context.started);
        if (!context.started) return;
        // Apply force if feet touching something
        var jumpForce = new Vector3(0f, this.jumpForce, 0f);
        if (feet.AddForce(-jumpForce))
            _body.AddForce(jumpForce);
    }

    public void Pan(InputAction.CallbackContext context)
    {
        var inputDelta = context.ReadValue<Vector2>();
        var panDelta = inputDelta / 100f * new Vector2(panSpeedX, -panSpeedY); // swap y pan direction
        
        _currentPan += panDelta;
        _currentPan.y = Mathf.Clamp(_currentPan.y, panMinY, panMaxY);

        _currentPan.x %= 360f;
        
    }

    public void Steer(InputAction.CallbackContext context) {
        if (!boat) return;
        var input = context.ReadValue<float>();
        steerDirection = input;
    }
}
