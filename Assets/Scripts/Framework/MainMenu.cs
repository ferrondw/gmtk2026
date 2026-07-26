using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yakanashe.Wiper;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transition transition;

    private void Start()
    {
        transition.Out(0.02f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("StartGame"))
        {
            transition.In(0.02f, () =>
            {
                SceneManager.LoadScene(1);
            });
        }
    }
}