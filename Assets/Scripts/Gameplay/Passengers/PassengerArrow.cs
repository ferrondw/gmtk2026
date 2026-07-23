using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yakapedia;

public class PassengerArrow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer innerArrow;

    private Vector2 _targetPosition = Vector2.zero;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void PointToDropoff(Passenger targetPassenger)
    {
        innerArrow.color = targetPassenger.ColorScheme.BaseColor;

        _targetPosition = PassengerDropoff.GetDropoffPosition(targetPassenger.DropoffId);
        transform.LookAt2D(_targetPosition);

        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (_targetPosition == Vector2.zero) return;
        transform.LookAt2D(_targetPosition);
    }

    public void StopPointing()
    {
        gameObject.SetActive(false);
        _targetPosition = Vector2.zero;
    }
}
