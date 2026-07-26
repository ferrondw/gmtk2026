using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yakanashe.Yautl;

public class BoatScrew : MonoBehaviour
{
    [SerializeField] private Boat boat;
    [SerializeField] private Transform screwTransform;
    [SerializeField] private float steerWeight = 10;

    private float _originRotation;

    private void Start()
    {
        _originRotation = screwTransform.localRotation.eulerAngles.z;
    }

    private void Update()
    {
        screwTransform.localRotation = Quaternion.Euler(0,0, _originRotation + (boat.InputVector.normalized.x * steerWeight));
    }
}
