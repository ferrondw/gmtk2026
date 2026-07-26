using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yakanashe.Yautl;

public class Boat : MonoBehaviour
{
    [Header("Speed and Steering")]
    [SerializeField] private float maxSpeed = 16f;
    [SerializeField] private float accelerationMultiplier = 12;
    [SerializeField] private float steerMultiplier = 2;
    [SerializeField] private float steerLimiterMultiplier = 2;

    [Header("Boosting")]
    [SerializeField] private float boostDuration = 4f;
    [SerializeField] private float boostSpeed = 25f;
    [SerializeField] private float boostSteerLimiterMultiplier = 20f;
    [SerializeField] private float boostAmountMultiplier = 0.7f;
    
    [Header("Jumping")]
    [SerializeField] private float jumpDuration = 2f;
    [SerializeField] private Vector3 jumpScale = new(1.4f, 1.4f, 1.4f);
    [SerializeField] private List<SpriteRenderer> spriteRenderers = new();
    [SerializeField] private int normalLayer;
    [SerializeField] private int jumpLayer = 3;

    [Header("Visuals")]
    [SerializeField] private Transform speedometer;
    [SerializeField] private Transform boatVisual;
    [SerializeField] private ParticleSystem boatWaterParticles;
    [SerializeField] private ParticleSystem boatFireParticles;

    [Header("Gameplay")]
    [SerializeField] private bool disableOnStart;

    [Header("Events")]
    [SerializeField] public UnityEvent OnBump;
    [SerializeField] public UnityEvent OnBoost;
    [SerializeField] public UnityEvent OnJump;
    [SerializeField] public UnityEvent OnStuck;

    private bool _locked;

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Vector2 _inputVector;
    private float _rotationAngle;
    private Vector2 _originPosition;

    private Coroutine _jumpCoroutine;
    private Coroutine _boostCoroutine;
    private float _startBoostTime;
    private bool _boosting;
    private int _currentBoostAmount;

    public Vector2 InputVector => _inputVector;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _originPosition = transform.position;
        boatWaterParticles.Play();
        SetLocked(disableOnStart);
    }

    public void SetLocked(bool newLock) => _locked = newLock;

    private void Update()
    {
        if (_locked) return;
        _inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        if (_locked) return;

        _rotationAngle -= _inputVector.x * steerMultiplier * Mathf.Clamp01(_rb.velocity.magnitude / (_boosting ? boostSteerLimiterMultiplier : steerLimiterMultiplier));
        _rb.MoveRotation(_rotationAngle);
        // speedometer.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(100f, -100f, Mathf.Clamp01(_rb.velocity.magnitude / minMaxSpeed.y)));

        var velocityUp = Vector2.Dot(transform.up, _rb.velocity);
        var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);
        _rb.velocity = (forwardVelocity * 0.98f) + (rightVelocity * 0.9f);

        if (!_boosting && velocityUp < -maxSpeed * 0.5f && _inputVector.y < 0) return;
        if (!_boosting && velocityUp > maxSpeed && _inputVector.y > 0) return;

        var engineForce = transform.up * (_boosting ? boostSpeed + (_currentBoostAmount * boostAmountMultiplier) : accelerationMultiplier * _inputVector.y);
        _rb.AddForce(engineForce, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BoostPanel"))
        {
            Boost();
            var boostCan = other.GetComponent<BoostCan>();
            if (boostCan != null) boostCan.Use();

            OnBoost.Invoke();
        }
        if (other.gameObject.CompareTag("JumpPanel"))
        {
            if (_jumpCoroutine != null) return;
            Jump();
            OnJump.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BoostPanel")) return;
        if (collision.gameObject.CompareTag("JumpPanel")) return;
        OnBump.Invoke();
    }

    public void Boost()
    {
        if (_boostCoroutine != null) StopCoroutine(_boostCoroutine);
        _boostCoroutine = StartCoroutine(nameof(BoostCoroutine));
    }

    public void Jump()
    {
        if (_jumpCoroutine != null) return;
        _jumpCoroutine = StartCoroutine(nameof(JumpCoroutine));
    }

    private IEnumerator BoostCoroutine()
    {
        _currentBoostAmount++;
        _startBoostTime = Time.time;
        _boosting = true;
        boatFireParticles.Play();

        while (Time.time - _startBoostTime < boostDuration)
        {
            yield return null;
        }
        
        StopBoost();
        
        yield return null;
    }

    private void StopBoost()
    {
        _boosting = false;
        boatFireParticles.Stop();
        _currentBoostAmount = 0;
    }

    private Coroutine JumpCoroutine()
    {
        _collider.enabled = false;
        foreach (var spriteRenderer in spriteRenderers) spriteRenderer.sortingOrder = jumpLayer;
        boatWaterParticles.Stop();

        boatVisual.ScaleTo(jumpScale, jumpDuration * 0.5f, EaseType.OutQuad).OnComplete(() =>
        {
            boatVisual.ScaleTo(Vector3.one, jumpDuration * 0.5f, EaseType.InQuad).OnComplete(() =>
            {
                var hit = Physics2D.Raycast(transform.position, Vector2.up, .1f, LayerMask.GetMask("Terrain"));
                if (hit)
                {
                    if (_boostCoroutine != null) StopCoroutine(_boostCoroutine);
                    StopBoost();

                    transform.position = _originPosition;
                    OnStuck.Invoke();
                }

                _collider.enabled = true;
                boatWaterParticles.Play();
                foreach (var spriteRenderer in spriteRenderers) spriteRenderer.sortingOrder = normalLayer;
                _jumpCoroutine = null;
            });
        });

        return null;
    }


    private void OnDisable() { StopAllCoroutines(); }
    private void OnDestroy() { StopAllCoroutines(); }
    private void OnApplicationQuit() { StopAllCoroutines(); }
}