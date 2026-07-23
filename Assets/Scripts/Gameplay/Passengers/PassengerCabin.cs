using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PassengerCabin : MonoBehaviour
{
    [Header("Rendering")]
    [SerializeField] private SpriteRenderer passengerRenderer;
    [SerializeField] private Sprite defaultPassengerTexture;

    private Passenger _currentPassenger;
    private Collider2D _collider;

    public bool HasPassenger => _currentPassenger != null;


    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        passengerRenderer.sprite = null;
    }

    public void Pickup(Passenger newPassenger)
    {
        if (HasPassenger)
        {
            Debug.Log("Player already has a passenger!");
            return;
        }

        _currentPassenger = newPassenger;

        Debug.Log("Passenger picked up!");
        Debug.Log("Delivery ID: " + _currentPassenger.DropoffId);

        passengerRenderer.sprite = defaultPassengerTexture;
        passengerRenderer.color = _currentPassenger.ColorScheme.BaseColor; // USE SHADER LATER
    }

    public void Launch()
    {
        _currentPassenger = null;
        passengerRenderer.sprite = null;
        // REMEMBER THAT IF PASSENGER MISSES IT SHOULD USE ITS ID TO DEACTIVATE ITS DROPOFF
    }
}
