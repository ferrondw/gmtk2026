using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yakapedia;

public class Onlooker : VisChecker
{
    [SerializeField] private string followerTag = "Player";

    private void FixedUpdate()
    {
        if (_isVisible == false) return;

        var followedTransform = GameObject.FindWithTag(followerTag).transform;
        transform.LookAt2D(followedTransform.position);
    }
}
