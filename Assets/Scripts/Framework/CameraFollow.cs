using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothTime;
    [SerializeField] private Transform target;
    
    private Camera _cam;
    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        _cam = GetComponent<Camera>();
    }

    private void Update()
    {
        var point = _cam.WorldToViewportPoint(target.position);
        var delta = target.position - _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        var destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, smoothTime);
    }
}