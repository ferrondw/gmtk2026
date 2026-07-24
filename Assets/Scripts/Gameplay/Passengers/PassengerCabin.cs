using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class PassengerCabin : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private Transform passengerMuzzle;
    [SerializeField] private GameObject defaultPassengerProjectile;
    [SerializeField] private string projectileContainerTag = "ProjectileContainer";
    [SerializeField] private PassengerArrow arrow;

    [Header("Mood")]
    [SerializeField] private float moodTimeout = 5f;
    [SerializeField] private int lowMoodDivision = 3;

    [Header("Rendering")]
    [SerializeField] private SpriteRenderer passengerRenderer;
    [SerializeField] private Sprite defaultPassengerTexture;

    [Header("Events")]
    [SerializeField] public UnityEvent OnFull = new();
    [SerializeField] public UnityEvent OnPickup = new();

    [SerializeField] public UnityEvent OnMoodChange = new();
    [SerializeField] public UnityEvent OnLowMood = new();
    [SerializeField] public UnityEvent OnPassengerLost = new();

    [SerializeField] public UnityEvent OnHoldLaunch = new();
    [SerializeField] public UnityEvent OnLaunch = new();

    private Passenger _currentPassenger;
    private Material _passengerMaterial;
    private Collider2D _collider;
    private Transform _projectileContainer;

    private int _currentMood;
    private int _maxMood;
    private WaitForSeconds _moodTimer;

    public bool HasPassenger => _currentPassenger != null;


    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        _projectileContainer = GameObject.FindGameObjectWithTag(projectileContainerTag).transform;
        _moodTimer = new WaitForSeconds(moodTimeout);

        passengerRenderer.sprite = null;
    }

    public void Pickup(Passenger newPassenger, Material newPassengerMaterial)
    {
        if (HasPassenger)
        {
            OnFull.Invoke();
            Debug.Log("Player already has a passenger!");
            return;
        }
        StopAllCoroutines();

        _currentPassenger = newPassenger;

        Debug.Log("Passenger picked up!");
        Debug.Log("Delivery ID: " + _currentPassenger.DropoffId);

        passengerRenderer.sprite = defaultPassengerTexture;
        _passengerMaterial = newPassengerMaterial;
        passengerRenderer.material = _passengerMaterial;

        _maxMood = newPassenger.MoodStates;
        _currentMood = _maxMood;

        StartCoroutine(MoodLoop());

        arrow.PointToDropoff(_currentPassenger);

        OnPickup.Invoke();
    }


    private IEnumerator MoodLoop()
    {
        yield return _moodTimer;
        ImpactMood();
    }

    public void ImpactMood(int moodDamage = 1)
    {
        StopAllCoroutines();

        Debug.Log("Passenger mood damage taken! Mood from " + _currentMood.ToString() + " to " + (_currentMood - moodDamage).ToString());

        _currentMood -= moodDamage;
        OnMoodChange.Invoke();

        if (_currentMood <= 0)
        {
            Launch();
            OnPassengerLost.Invoke();
            return;
        }
        if (_currentMood == Mathf.FloorToInt(_maxMood / lowMoodDivision)) OnLowMood.Invoke();

        StartCoroutine(MoodLoop());
    }


    private void Update()
    {
        if (_currentPassenger == null) return;
        if (Input.GetKeyDown(KeyCode.Space)) OnHoldLaunch.Invoke();
        if (Input.GetKeyUp(KeyCode.Space)) Launch();
    }

    public void Launch()
    {
        StopAllCoroutines();

        passengerRenderer.sprite = null;
        arrow.StopPointing();

        var projectile = Instantiate(defaultPassengerProjectile, passengerMuzzle.position, passengerMuzzle.rotation, _projectileContainer.transform);
        var projectileComponent = projectile.GetComponent<PassengerBullet>();

        projectileComponent.SetPassenger(_currentPassenger);
        projectileComponent.SetMaterial(_passengerMaterial);

        _currentPassenger = null;

        OnLaunch.Invoke();
    }


    private void OnDisable() { StopAllCoroutines(); }
    private void OnDestroy() { StopAllCoroutines(); }
    private void OnApplicationQuit() { StopAllCoroutines(); }
}
