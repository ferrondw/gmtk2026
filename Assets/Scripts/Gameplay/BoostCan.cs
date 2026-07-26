using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoostCan : MonoBehaviour
{
    [SerializeField] private float rechargeTime = 1f;
    [SerializeField] private UnityEvent OnPlayerDetected = new();
    [SerializeField] private UnityEvent OnRecharge = new();

    private const string PlayerTag = "Player";

    private WaitForSeconds _timer;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _timer = new WaitForSeconds(rechargeTime);
    }

    public void Use()
    {
        _collider.enabled = false;
        StartCoroutine(Recharge());
        OnPlayerDetected.Invoke();
    }

    private IEnumerator Recharge()
    {
        yield return _timer;
        _collider.enabled = true;
        OnRecharge.Invoke();
    }

    private void OnDisable() { StopAllCoroutines(); }
    private void OnDestroy() { StopAllCoroutines(); }
    private void OnApplicationQuit() { StopAllCoroutines(); }
}
