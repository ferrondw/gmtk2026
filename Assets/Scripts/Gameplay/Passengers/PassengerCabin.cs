using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class PassengerCabin : MonoBehaviour
{
    [SerializeField] private Transform passengerMuzzle;
    [SerializeField] private GameObject defaultPassengerProjectile;
    [SerializeField] private string projectileContainerTag = "ProjectileContainer";
    [SerializeField] private PassengerArrow arrow;

    [Header("Rendering")]
    [SerializeField] private SpriteRenderer passengerRenderer;
    [SerializeField] private Sprite defaultPassengerTexture;

    [Header("Events")]
    [SerializeField] public UnityEvent OnFull = new();
    [SerializeField] public UnityEvent OnPickup = new();
    [SerializeField] public UnityEvent OnHoldLaunch = new();
    [SerializeField] public UnityEvent OnLaunch = new();

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
            OnFull.Invoke();
            Debug.Log("Player already has a passenger!");
            return;
        }

        _currentPassenger = newPassenger;

        Debug.Log("Passenger picked up!");
        Debug.Log("Delivery ID: " + _currentPassenger.DropoffId);

        passengerRenderer.sprite = defaultPassengerTexture;
        passengerRenderer.color = _currentPassenger.ColorScheme.BaseColor; // USE SHADER LATER

        arrow.PointToDropoff(_currentPassenger);

        OnPickup.Invoke();
    }


    private void Update()
    {
        if (_currentPassenger == null) return;
        if (Input.GetKeyDown(KeyCode.Space)) OnHoldLaunch.Invoke();
        if (Input.GetKeyUp(KeyCode.Space)) Launch();
    }

    public void Launch()
    {
        passengerRenderer.sprite = null;
        arrow.StopPointing();

        var projectile = Instantiate(defaultPassengerProjectile, passengerMuzzle.position, passengerMuzzle.rotation, _projectileContainer.transform);
        var projectileComponent = projectile.GetComponent<PassengerBullet>();

        projectileComponent.Passenger = _currentPassenger;
        _currentPassenger = null;

        OnLaunch.Invoke();
    }
}
