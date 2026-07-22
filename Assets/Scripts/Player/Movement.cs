using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxSpeed = new(3.5f, 9f);
    [SerializeField] private Vector2 minMaxDriftMultiplier = new(0.8f, 0.98f);
    [SerializeField] private float accelerationMultiplier;
    [SerializeField] private float steerMultiplier;
    [SerializeField] private float steerLimiterMultiplier;
    [SerializeField] private float postDriftBoostMultiplier;
    [SerializeField] private ParticleSystem driftParticles;

    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform speedometer;

    private Rigidbody2D _rb;
    private Vector2 _inputVector;
    private float _rotationAngle;
    private float _driftMultiplier;
    private float _maxSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _driftMultiplier = minMaxDriftMultiplier.x;
        _maxSpeed = minMaxSpeed.y;
    }

    private void Update()
    {
        _inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            driftParticles.Play();
            _driftMultiplier = minMaxDriftMultiplier.y;
            _maxSpeed = minMaxSpeed.x;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            driftParticles.Stop();
            _driftMultiplier = minMaxDriftMultiplier.x;
            _maxSpeed = minMaxSpeed.y;
            _rb.AddForce(transform.up * postDriftBoostMultiplier, ForceMode2D.Force);
        }
    }

    public void FixedUpdate()
    {
        _rotationAngle -= _inputVector.x * steerMultiplier * Mathf.Clamp01(_rb.velocity.magnitude / steerLimiterMultiplier);
        _rb.MoveRotation(_rotationAngle);
        speedometer.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(100f, -100f, Mathf.Clamp01(_rb.velocity.magnitude / minMaxSpeed.y)));
        
        var velocityUp = Vector2.Dot(transform.up, _rb.velocity);
        if (velocityUp < -_maxSpeed * 0.5f && _inputVector.y < 0) return;
        if (velocityUp > _maxSpeed && _inputVector.y > 0) return;
        
        var engineForce = transform.up * (accelerationMultiplier * _inputVector.y);
        _rb.AddForce(engineForce, ForceMode2D.Force);

        frontLeftWheel.localEulerAngles = new Vector3(0, 0, _rb.angularVelocity * 0.4f);
        frontRightWheel.localEulerAngles = new Vector3(0, 0, _rb.angularVelocity * 0.4f);

        var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);
        _rb.velocity = forwardVelocity + rightVelocity * _driftMultiplier;
    }
}