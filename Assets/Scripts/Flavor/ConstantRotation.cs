using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : VisChecker
{
    [SerializeField] private bool lockRotation;
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        if (_isVisible == false) return;
        if (lockRotation)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            return;
        }

        transform.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z + speed);
    }
}
