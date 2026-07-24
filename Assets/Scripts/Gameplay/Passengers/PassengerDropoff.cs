using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class PassengerDropoff : MonoBehaviour
{
    [SerializeField] public string DropoffId;
    [SerializeField] private int missCountdownPenalty = 15;

    [Header("Rendering")]
    [SerializeField] private SpriteRenderer innerRenderer;
    [SerializeField] private SpriteRenderer outerRenderer;
    [SerializeField] private SpriteRenderer targetRenderer;

    [Header("Events")]
    [SerializeField] public UnityEvent OnActivate = new();
    [SerializeField] public UnityEvent OnDeliver = new();
    [SerializeField] public UnityEvent OnMiss = new();

    public static List<PassengerDropoff> instances = new List<PassengerDropoff>();

    private Collider2D _collider;
    private Timer _timer;

    private void Start()
    {
        instances.Add(this);

        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();

        Deactivate();
    }

    public void Activate(Color zoneColor)
    {
        innerRenderer.enabled = true;
        outerRenderer.enabled = true;
        targetRenderer.enabled = true;

        innerRenderer.color = zoneColor;
        targetRenderer.color = zoneColor;

        _collider.enabled = true;

        OnActivate.Invoke();
    }

    public void Deliver(Passenger deliveredPassenger)
    {
        if (deliveredPassenger.DropoffId != DropoffId) return;

        _timer.AddTime(20); // MAKE PASSENGERS CONTAIN MORE OR LESS TIME LATER TO PARSE IN

        Deactivate();

        OnDeliver.Invoke();
        Debug.Log("Passenger delivered!");
    }

    public void Miss()
    {
        _timer.RemoveTime(missCountdownPenalty);

        Deactivate();

        OnMiss.Invoke();
        Debug.Log("Passenger Missed!");
    }

    private void Deactivate()
    {
        innerRenderer.enabled = false;
        outerRenderer.enabled = false;
        targetRenderer.enabled = false;

        _collider.enabled = false;
    }


    public static string GetRandomDropoffId()
    {
        var number = Random.Range(0, instances.Count);
        return instances[number].DropoffId;
    }

    public static PassengerDropoff GetPassengerDropoff(string id)
    {
        foreach (var instance in instances)
        {
            if (instance.DropoffId == id) return instance;
        }

        Debug.Log("No dropoff found with ID " + id + ", returning null");
        return null;
    }

    public static Vector2 GetDropoffPosition(string id)
    {
        foreach (var instance in instances)
        {
            if (instance.DropoffId == id) return instance.transform.position;
        }

        Debug.Log("No dropoff found with ID " + id + ", returning Vector2(0, 0)");
        return Vector2.zero;
    }
}
