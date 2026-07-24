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

    public Passenger Passenger;

    private bool _hit;
    private Rigidbody2D _rigidBody;
    private SpriteRenderer _sprite;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.isKinematic = true;
        _rigidBody.velocity = transform.right * speed;

        _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = Passenger.ColorScheme.BaseColor; // USE SHADER

        OnStart.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var dropoff = collision.GetComponent<PassengerDropoff>();
        if (dropoff != null)
        {
            dropoff.Deliver(Passenger);
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

        var dropoff = PassengerDropoff.GetPassengerDropoff(Passenger.DropoffId);
        dropoff.Miss();

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
