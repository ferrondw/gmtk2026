using UnityEngine;
using Yakanashe.Yautl;

public class Boat : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 16f;
    [SerializeField] private float boostSpeed = 25f;
    [SerializeField] private float boostDuration = 4f;
    [SerializeField] private float boostSteerLimiterMultiplier = 4f;
    [SerializeField] private float accelerationMultiplier = 12;
    [SerializeField] private float steerMultiplier = 2;
    [SerializeField] private float steerLimiterMultiplier = 2;

    [SerializeField] private Transform speedometer;
    [SerializeField] private Transform boatVisual;
    [SerializeField] private ParticleSystem boatWaterParticles;
    [SerializeField] private ParticleSystem boatFireParticles;

    private Rigidbody2D _rb;
    private Vector2 _inputVector;
    private float _rotationAngle;

    private Coroutine _boostCoroutine;
    private bool _boosting;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        boatWaterParticles.Play();
    }

    private void Update()
    {
        _inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        _rotationAngle -= _inputVector.x * steerMultiplier * Mathf.Clamp01(_rb.velocity.magnitude / (_boosting ? boostSteerLimiterMultiplier : steerLimiterMultiplier));
        _rb.MoveRotation(_rotationAngle);
        // speedometer.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(100f, -100f, Mathf.Clamp01(_rb.velocity.magnitude / minMaxSpeed.y)));

        var velocityUp = Vector2.Dot(transform.up, _rb.velocity);
        var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);
        _rb.velocity = (forwardVelocity * 0.98f) + (rightVelocity * 0.9f);

        if (!_boosting && velocityUp < -maxSpeed * 0.5f && _inputVector.y < 0) return;
        if (!_boosting && velocityUp > maxSpeed && _inputVector.y > 0) return;

        var engineForce = transform.up * (_boosting ? boostSpeed : accelerationMultiplier * _inputVector.y);
        _rb.AddForce(engineForce, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("BoostPanel")) return;

        if (_boostCoroutine != null) StopCoroutine(_boostCoroutine);
        _boostCoroutine = StartCoroutine(nameof(Boost));
    }

    public Coroutine Boost()
    {
        _boosting = true;
        boatWaterParticles.Stop();
        boatFireParticles.Play();
        boatVisual.ScaleTo(new Vector3(1.4f, 1.4f, 1.4f), boostDuration * 0.3f, EaseType.OutQuad).OnComplete(() =>
        {
            boatVisual.ScaleTo(Vector3.one, boostDuration * 0.3f, EaseType.InQuad).OnComplete(() =>
            {
                boatWaterParticles.Play();
            });
        });

        transform.ScaleTo(Vector3.one, boostDuration).OnComplete(() =>
        {
            _boosting = false;
            boatFireParticles.Stop();
        });
        return null;
    }
}