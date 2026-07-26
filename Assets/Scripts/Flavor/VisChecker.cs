using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisChecker : MonoBehaviour
{
    [SerializeField] public UnityEvent OnInvisible = new(); 
    [SerializeField] public UnityEvent OnVisible = new(); 
    public bool _isVisible = false;

    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        _isVisible = renderer.isVisible;
    }

    private void OnBecameInvisible()
    {
        _isVisible = false;
        OnInvisible.Invoke();
    }
    private void OnBecameVisible()
    {
        _isVisible = true;
        OnVisible.Invoke();
    }
}
