using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Yakanashe.Yautl;

public class ScoreVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private string gameStateTag = "GameState";

    [Header("Start Animation")]
    [SerializeField] private bool startHidden = true;
    [SerializeField] private float startScaleTime = .25f;
    [SerializeField] private EaseType scaleEaseType = EaseType.InCirc;

    [Header("Events")]
    [SerializeField] public UnityEvent OnAddedScore = new();

    private float _lastScoreUpdate;
    private RectTransform _rectTransform;
    private bool _isHidden;

    private void Start()
    {
        if (startHidden)
        {
            _isHidden = true;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.localScale = Vector2.zero;
            _isHidden = true;
        }

        var score = GameObject.FindGameObjectWithTag(gameStateTag).GetComponent<Score>();
        score.OnScoreUpdate.AddListener(UpdateScore);
    }

    private void UpdateScore(int newScore)
    {
        if (_isHidden)
        {
            _rectTransform.ScaleTo(Vector2.one, startScaleTime, scaleEaseType);
            _isHidden = false;
        }

        // ADD SOMETHING LIKE ADDED SCORE ANIMATION (TWEENING?) WITH _lastScoreUpdate

        scoreLabel.text = "$ " + newScore.ToString();
        _lastScoreUpdate = newScore;
    }
}
