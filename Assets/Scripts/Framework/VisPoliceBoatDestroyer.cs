using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisPoliceBoatDestroyer : MonoBehaviour
{
    [SerializeField] public GameObject boatObject;

    private void OnBecameInvisible()
    {
        WaveSpawner.Instance.DestroyBoat(boatObject);
        Destroy(boatObject);
    }
}
