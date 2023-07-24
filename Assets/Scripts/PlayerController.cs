using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _body;
    private Collider _collider;
    private bool _isOnGround;

    [SerializeField] public float jumpImpulse;
    [SerializeField] public float moveForce;
    [SerializeField] public float panSpeedX;
    [SerializeField] public float panSpeedY;
    [SerializeField] public float panMinY;
    [SerializeField] public float panMaxY;
    [SerializeField] public GameObject camTargetPosition;
    [SerializeField] public GameObject camTarget;


    private Vector3 _moveInputDirection;
    private Vector2 _currentPan;

    [SerializeField] public float smoothTime;
    private float _currentVelocity;
    
    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        MoveUpdate();
        CamDirUpdate();
    }

    private void CamDirUpdate()
    {
        camTarget.transform.rotation = Quaternion.Euler(_currentPan.y, _currentPan.x, 0f);
        camTarget.transform.position = camTargetPosition.transform.position;
    }

    private void MoveUpdate()
    {
        if (_moveInputDirection.sqrMagnitude == 0) return;
        
        // target direction is relative to camTarget direction. Rotate it:
        var actualDirection = Quaternion.Euler(0f, camTarget.transform.eulerAngles.y, 0f) * _moveInputDirection;
        // Get the Y angle
        var targetAngle = Mathf.Atan2(actualDirection.x, actualDirection.z) * Mathf.Rad2Deg;
        // Smooth it
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        // Set the character angle
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        // Force direction. Not smoothed
        var moveForceVec = actualDirection * moveForce;
        // Apply force
        _body.AddForce(moveForceVec * Time.deltaTime);
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
        _body.AddRelativeForce(0f, jumpImpulse, 0f, ForceMode.Impulse);
    }

    public void Pan(InputAction.CallbackContext context)
    {
        var inputDelta = context.ReadValue<Vector2>();
        var panDelta = inputDelta / 100f * new Vector2(panSpeedX, -panSpeedY); // swap y pan direction
        
        _currentPan += panDelta;
        _currentPan.y = Mathf.Clamp(_currentPan.y, panMinY, panMaxY);

        _currentPan.x %= 360f;
        
    }
}
