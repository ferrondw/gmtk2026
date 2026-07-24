using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numericTimer;
    [SerializeField] private RectTransform clockArrow;
    [SerializeField] [Range(-360f, 0)] private float rotationPerSecond = -30f;
    [SerializeField] private string timerTag = "Timer";

    [Header("Events")]
    [SerializeField] public UnityEvent OnLoseTime;
    [SerializeField] public UnityEvent OnAddedTime;

    private float _lastTimeUpdate;

    private void Start()
    {
        var timer = GameObject.FindGameObjectWithTag(timerTag).GetComponent<Timer>();
        timer.OnTimeUpdate.AddListener(DisplayTime);
    }

    private void DisplayTime(float newTime) // ADD EFFECTS OF ADDING VS LOSING TIME LATER
    {
        numericTimer.text = newTime.ToString("f1");

        if (_lastTimeUpdate == 0f)
        {
            _lastTimeUpdate = newTime;
            return;
        }

        if (_lastTimeUpdate < newTime)
        {
            _lastTimeUpdate = newTime;
            clockArrow.rotation = Quaternion.Euler(Vector3.zero);
            OnAddedTime.Invoke();

            return;
        }

        _lastTimeUpdate = newTime;
        OnLoseTime.Invoke();

        if (clockArrow.rotation.eulerAngles.z + rotationPerSecond < -360 || clockArrow.rotation.eulerAngles.z + rotationPerSecond > 360)
        {
            clockArrow.rotation = Quaternion.Euler(Vector3.zero);
            return;
        }
        clockArrow.rotation = Quaternion.Euler(0, 0, clockArrow.rotation.eulerAngles.z + rotationPerSecond);
    }
}
