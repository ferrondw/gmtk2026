using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEngineGrumble : MonoBehaviour
{
    [SerializeField] private Transform engineTransform;
    [SerializeField] private float speed = .25f;
    [SerializeField] private float displacement = .02f;

    private Vector3 _origin;
    private float _time;

    private void Start()
    {
        _origin = engineTransform.localPosition;
    }

    private void Update()
    {
        if (_time < speed)
        {
            _time += Time.deltaTime;
            return;
        }

        _time = 0;
        engineTransform.transform.localPosition = new Vector3(_origin.x + Random.Range(-displacement, displacement), _origin.y + Random.Range(-displacement, displacement), _origin.z);
    }
}
