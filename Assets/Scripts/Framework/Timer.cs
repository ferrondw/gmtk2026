using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("Start Signal")]
    [SerializeField] private float startWait = 1f;
    [SerializeField] private int timeToStart = 4;
    [SerializeField] private float startTimePerSecond = 1f;

    [Header("CountDown Loop")]
    [SerializeField] private float startTime = 60;
    [SerializeField] private float timePerSecond = .2f;
    [SerializeField] private float minTimePerSecond = .05f; // ADD EVENTUAL DROPOFF

    [Header("Start Events")]
    [SerializeField] public UnityEvent<int> OnTimeStartUpdate = new();
    [SerializeField] public UnityEvent OnBegin = new();
    [SerializeField] public UnityEvent OnStartDeplete = new();
    [SerializeField] public UnityEvent OnStarted = new();

    [Header("Loop Events")]
    [SerializeField] public UnityEvent<float> OnTimeUpdate = new();
    [SerializeField] public UnityEvent OnDeplete = new();
    [SerializeField] public UnityEvent OnAdd = new();
    [SerializeField] public UnityEvent OnLose = new();

    private const float TimeIncrement = .1f;

    private float _currentTime;
    private int _loops;
    private WaitForSeconds _currentTimePerSecond;

    private void Start()
    {
        _currentTime = startTime;
        SetNewCountdownSeconds(timePerSecond);
        StartCoroutine(TimeToStartCoroutine());
    }

    public void SetNewCountdownSeconds(float newTime) => _currentTimePerSecond = new WaitForSeconds(newTime); // Set new amount of how much each second of the countdown lasts

    private IEnumerator TimeToStartCoroutine()
    {
        yield return new WaitForSeconds(startWait);

        Debug.Log("Starting start signal timer");

        OnBegin.Invoke();
        for (int time = timeToStart; time > 0; time--)
        {
            yield return new WaitForSeconds(startTimePerSecond);
            OnTimeStartUpdate.Invoke(time);
            OnStartDeplete.Invoke();
        }
        OnStarted.Invoke();

        Debug.Log("Starting countdown!");

        StartCoroutine(CoreTimerLoop());
    }
    private IEnumerator CoreTimerLoop()
    {
        yield return _currentTimePerSecond;

        if (_currentTime - TimeIncrement <= 0)
        {
            Lose();
            yield break;
        }

        _currentTime -= TimeIncrement;
        _loops++;

        OnTimeUpdate.Invoke(_currentTime);
        OnDeplete.Invoke();

        StartCoroutine(CoreTimerLoop());
    }

    private void Lose()
    {
        OnLose.Invoke();
        OnTimeUpdate.Invoke(0f);
        Debug.Log("Game over!");
    }


    public void AddTime(int addedTime)
    {
        StopAllCoroutines();
        _currentTime += (float)addedTime;
        OnTimeUpdate.Invoke(_currentTime);
        StartCoroutine(CoreTimerLoop());
    }

    public void RemoveTime(int addedTime)
    {
        StopAllCoroutines();

        if (_currentTime - TimeIncrement <= 0)
        {
            Lose();
            return;
        }

        _currentTime -= (float)addedTime;
        OnTimeUpdate.Invoke(_currentTime);
        StartCoroutine(CoreTimerLoop());
    }


    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
