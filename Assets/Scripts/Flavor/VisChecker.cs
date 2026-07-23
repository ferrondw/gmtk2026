using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisChecker : MonoBehaviour
{
    public bool _isVisible = false;

    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        _isVisible = renderer.isVisible;
    }

    private void OnBecameInvisible() => _isVisible = false;
    private void OnBecameVisible() => _isVisible = true;
}
