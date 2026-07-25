using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    
    [SerializeField] private float smoothTime;
    [SerializeField] private Collider2D bounds;
    [SerializeField] private Transform target;
    
    private Camera _cam;
    private Vector3 _velocity = Vector3.zero;
    
    private Vector3 _shakeOffset = Vector3.zero;
    private float _timeAtCurrentFrame;
    private float _timeAtLastFrame;
    private float _fakeDelta;
    private Rigidbody2D _targetRigidbody;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _cam = GetComponent<Camera>();
        _targetRigidbody = target.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _timeAtCurrentFrame = Time.realtimeSinceStartup;
        _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
        _timeAtLastFrame = _timeAtCurrentFrame;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        
        var delta = target.position - _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));
        var destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, smoothTime);
        transform.position += _shakeOffset;
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, 12 + _targetRigidbody.velocity.magnitude * 0.5f, Time.deltaTime * 2);
        ClampCamera();
    }

    public static void Shake(float duration, float amount)
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    private IEnumerator cShake(float duration, float amount)
    {
        while (duration > 0)
        {
            _shakeOffset = Random.insideUnitSphere * amount;
            duration -= _fakeDelta;
            yield return null;
        }

        _shakeOffset = Vector3.zero;
    }
    
    private void ClampCamera()
    {
        if (!bounds) return;
        
        var cameraHeight = _cam.orthographicSize * 2f;
        var cameraWidth = cameraHeight * _cam.aspect;

        var bounds1 = bounds.bounds;
        var minPosition = bounds1.min;
        var maxPosition = bounds1.max;

        var position = transform.position;
        var clampedX = Mathf.Clamp(position.x, minPosition.x + cameraWidth / 2f, maxPosition.x - cameraWidth / 2f);
        var clampedY = Mathf.Clamp(position.y, minPosition.y + cameraHeight / 2f, maxPosition.y - cameraHeight / 2f);
        position = new Vector3(clampedX, clampedY, position.z);
        transform.position = position;
    }
}
