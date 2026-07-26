using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yakanashe.Wiper;
using Yakanashe.Yautl;
using EaseType = Yakanashe.Yautl.EaseType;

public class TimerJuice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Color originalColor;

    [Header("Add")]
    [SerializeField] private float addDuration = .5f;
    [SerializeField] private Vector2 addScale = new Vector2(1f, 1.5f);
    [SerializeField] private Color addColor;
    [SerializeField] private EaseType addScaleEaseType;
    [SerializeField] private EaseType addColorEaseType;

    [Header("Remove")]
    [SerializeField] private float removeDuration = .5f;
    [SerializeField] private Vector2 removeScale = new Vector2(1.2f, .8f);
    [SerializeField] private Color removeColor;
    [SerializeField] private EaseType removeScaleEaseType;
    [SerializeField] private EaseType removeColorEaseType;

    public void Add()
    {
        _timerText.rectTransform.localScale = addScale;
        _timerText.rectTransform.ScaleTo(Vector3.one, addDuration, addScaleEaseType);

        _timerText.color = addColor;
        _timerText.ColorTo(originalColor, addDuration, addColorEaseType);
    }

    public void Remove()
    {
        _timerText.rectTransform.localScale = removeScale;
        _timerText.rectTransform.ScaleTo(Vector3.one, removeDuration, removeScaleEaseType);

        _timerText.color = removeColor;
        _timerText.ColorTo(originalColor, removeDuration, removeColorEaseType);
    }
}
