using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PassengerCabin : MonoBehaviour
{
    [SerializeField] private Transform passengerMuzzle;
    [SerializeField] private GameObject defaultPassengerProjectile;
    [SerializeField] private string projectileContainerTag = "ProjectileContainer";

    [Header("Rendering")]
    [SerializeField] private SpriteRenderer passengerRenderer;
    [SerializeField] private Sprite defaultPassengerTexture;

    private Passenger _currentPassenger;
    private Collider2D _collider;
    private Transform _projectileContainer;

    public bool HasPassenger => _currentPassenger != null;


    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        _projectileContainer = GameObject.FindGameObjectWithTag(projectileContainerTag).transform;

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


    private void Update()
    {
        if (_currentPassenger == null) return;
        if (Input.GetKeyUp(KeyCode.Space)) Launch();
    }

    public void Launch()
    {
        passengerRenderer.sprite = null;

        // Launch fx

        var projectile = Instantiate(defaultPassengerProjectile, passengerMuzzle.position, passengerMuzzle.rotation, _projectileContainer.transform);
        var projectileComponent = projectile.GetComponent<PassengerBullet>();

        projectileComponent.Passenger = _currentPassenger;
        _currentPassenger = null;
    }
}
