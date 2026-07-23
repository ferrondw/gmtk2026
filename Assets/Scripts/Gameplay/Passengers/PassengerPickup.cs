using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PassengerPickup : MonoBehaviour
{
    [Header("Rendering")]
    [SerializeField] private SpriteRenderer passengerRenderer;
    [SerializeField] private SpriteRenderer zoneRenderer;
    [SerializeField] private SpriteRenderer outerZoneRenderer;
    [SerializeField] private PassengerColorSchemeCollection colors;

    private PassengerColorScheme _currentPassengerColorScheme;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        //passengerRenderer.sprite = null;
        Activate();
    }

    private void Activate()
    {
        _currentPassengerColorScheme = colors.GetRandomColorScheme();
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
    }

    private void Deactivate() // Deactivate zone and start timer to activate
    {
        return;
    }
}
