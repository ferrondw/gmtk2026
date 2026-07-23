using System;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 16f;
    [SerializeField] private float boostSpeed = 25f;
    [SerializeField] private float boostDuration = 4f;
    [SerializeField] private float accelerationMultiplier;
    [SerializeField] private float steerMultiplier;
    [SerializeField] private float steerLimiterMultiplier;
    
    [SerializeField] private Transform speedometer;
    [SerializeField] private Transform boatVisual;

    private Rigidbody2D _rb;
    private Vector2 _inputVector;
    private float _rotationAngle;
    private float boostRemaining;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        // boost
        
    }

    private void FixedUpdate()
    {
        _rotationAngle -= _inputVector.x * steerMultiplier * Mathf.Clamp01(_rb.velocity.magnitude / steerLimiterMultiplier);
        _rb.MoveRotation(_rotationAngle);
        // speedometer.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(100f, -100f, Mathf.Clamp01(_rb.velocity.magnitude / minMaxSpeed.y)));

        var velocityUp = Vector2.Dot(transform.up, _rb.velocity);
        var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);
        _rb.velocity = (forwardVelocity * 0.98f) + (rightVelocity * 0.9f);

        if (velocityUp < -maxSpeed * 0.5f && _inputVector.y < 0) return;
        if (velocityUp > maxSpeed && _inputVector.y > 0) return;

        var engineForce = transform.up * (accelerationMultiplier * _inputVector.y);
        _rb.AddForce(engineForce, ForceMode2D.Force);
    }
}