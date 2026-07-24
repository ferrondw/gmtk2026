using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class PassengerPickup : MonoBehaviour
{
    [SerializeField] private float minResetTime = 8f;
    [SerializeField] private float maxResetTime = 15f;
    [SerializeField] private float stayTime = 20f;
    [SerializeField] private int extraTimeAdded = 3;

    [Header("Rendering")]
    [SerializeField] private SpriteRenderer passengerRenderer;
    [SerializeField] private SpriteRenderer zoneRenderer;
    [SerializeField] private SpriteRenderer outerZoneRenderer;
    [SerializeField] private PassengerColorSchemeCollection colors;

    [Header("Events")]
    [SerializeField] public UnityEvent OnActivate = new();
    [SerializeField] public UnityEvent OnDeactivate = new();
    [SerializeField] public UnityEvent OnPickup = new();

    private bool _isActive;
    private Coroutine _currentStayCoroutine;

    private PassengerColorScheme _currentPassengerColorScheme;
    private Collider2D _collider;
    private Timer _timer;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();

        Activate();
    }

    private void Activate()
    {
        passengerRenderer.enabled = true;
        zoneRenderer.enabled = true;
        outerZoneRenderer.enabled = true;

        _collider.enabled = true;

        _currentPassengerColorScheme = colors.GetRandomColorScheme();
        passengerRenderer.color = _currentPassengerColorScheme.BaseColor; // USE SHADER LATER

        _isActive = true;

        OnActivate.Invoke();

        Debug.Log("Passenger " + gameObject.name + " appeared!");
    }

    private void OnBecameVisible() => StopStayTimer();

    private void StopStayTimer()
    {
        if (_currentStayCoroutine == null) return;
        StopCoroutine(_currentStayCoroutine);

        Debug.Log("Stopping stay coroutine on passenger pickup: " + gameObject.name);
    }

    private void OnBecameInvisible()
    {
        if (_isActive == false) return;
        _currentStayCoroutine = StartCoroutine(StayTimeCoroutine());

        Debug.Log("Starting stay coroutine on passenger pickup: " + gameObject.name);
    }

    private IEnumerator StayTimeCoroutine()
    {
        yield return new WaitForSeconds(stayTime);
        Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var cabin = collision.GetComponent<PassengerCabin>();
        if (cabin == null) return;
        if (cabin.HasPassenger) return;

        var id = PassengerDropoff.GetRandomDropoffId();
        var dropoff = PassengerDropoff.GetPassengerDropoff(id);
        dropoff.Activate(_currentPassengerColorScheme.BaseColor);

        var newPassenger = new Passenger(id, _currentPassengerColorScheme);
        cabin.Pickup(newPassenger);

        _timer.AddTime(extraTimeAdded);

        Deactivate();
        StopStayTimer();
    }

    private void Deactivate()
    {
        passengerRenderer.enabled = false;
        zoneRenderer.enabled = false;
        outerZoneRenderer.enabled = false;

        _collider.enabled = false;
        _isActive = false;

        StartCoroutine(ResetTimeCoroutine());

        OnDeactivate.Invoke();

        Debug.Log("Passenger " + gameObject.name + " disappeared");
    }

    private IEnumerator ResetTimeCoroutine()
    {
        var resetTime = Random.Range(minResetTime, maxResetTime);
        yield return new WaitForSeconds(resetTime);
        Activate();
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
