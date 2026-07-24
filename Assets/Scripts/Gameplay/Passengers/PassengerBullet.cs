using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class PassengerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float stayTime = 1f;

    [Header("Events")]
    [SerializeField] public UnityEvent OnHitDropoff = new();
    [SerializeField] public UnityEvent OnHitPolice = new();
    [SerializeField] public UnityEvent OnStart = new();

    private Passenger _passenger;
    private PassengerDropoff _dropoff;

    private bool _hit;
    private Rigidbody2D _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.isKinematic = true;
        _rigidBody.velocity = transform.right * speed;

        OnStart.Invoke();
    }

    public void SetPassenger(Passenger newPassenger)
    {
        _passenger = newPassenger;
        _dropoff = PassengerDropoff.GetPassengerDropoff(_passenger.DropoffId);
        _dropoff.OnActivate.AddListener(() => 
        {
            StopAllCoroutines();
            _hit = true;
        });
    }

    public void SetMaterial(Material newMaterial)
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.material = newMaterial;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var dropoff = collision.GetComponent<PassengerDropoff>();
        if (dropoff == _dropoff)
        {
            dropoff.Deliver(_passenger);
            _hit = true;

            OnHitDropoff.Invoke();

            Destroy(gameObject);
            return;
        }
    }

    private void OnBecameVisible() => StopAllCoroutines();

    private void OnBecameInvisible()
    {
        if (_hit) return;
        StopAllCoroutines();
        StartCoroutine(StayTimeCoroutine());
    }

    private IEnumerator StayTimeCoroutine()
    {
        yield return new WaitForSeconds(stayTime);
        _dropoff.Miss();

        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
