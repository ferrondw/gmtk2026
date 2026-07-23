using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PassengerPickup : MonoBehaviour
{
    [SerializeField] private float minResetTime = 8f;
    [SerializeField] private float maxResetTime = 15f;
    [SerializeField] private float stayTime = 20f;

    [Header("Rendering")]
    [SerializeField] private SpriteRenderer passengerRenderer;
    [SerializeField] private SpriteRenderer zoneRenderer;
    [SerializeField] private SpriteRenderer outerZoneRenderer;
    [SerializeField] private PassengerColorSchemeCollection colors;

    private bool _isActive;
    private Coroutine _currentStayCoroutine;

    private PassengerColorScheme _currentPassengerColorScheme;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

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

        Deactivate();
        StopStayTimer();
    }

    private void Deactivate() // Deactivate zone and start timer to activate
    {
        passengerRenderer.enabled = false;
        zoneRenderer.enabled = false;
        outerZoneRenderer.enabled = false;

        _collider.enabled = false;
        _isActive = false;

        StartCoroutine(ResetTimeCoroutine());

        Debug.Log("Passenger " + gameObject.name + " disappeared");
    }

    private IEnumerator ResetTimeCoroutine()
    {
        var resetTime = Random.Range(minResetTime, maxResetTime);
        yield return new WaitForSeconds(resetTime);
        Activate();
    }
}
