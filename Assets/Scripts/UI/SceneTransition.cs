using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yakanashe.Wiper;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Transition transition;
    [SerializeField] private float duration = 0.02f;

    private void Start()
    {
        transition.Out(0.02f);
    }

    public void TransitionTo(int scene)
    {
        transition.In(0.02f, () => { SceneManager.LoadScene(scene); });
    }

    public void TransitionTo(string scene)
    {
        transition.In(0.02f, () => { SceneManager.LoadScene(scene); });
    }
}
