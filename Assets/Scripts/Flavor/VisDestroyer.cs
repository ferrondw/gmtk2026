using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisDestroyer : MonoBehaviour
{
    [SerializeField] public GameObject destroyObject;

    private void OnBecameInvisible() => Destroy(destroyObject);
}
