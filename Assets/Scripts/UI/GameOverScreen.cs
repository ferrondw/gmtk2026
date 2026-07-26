using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yakanashe.Yautl;
using static UnityEngine.Rendering.BoolParameter;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup hudGroup;
    [SerializeField] private float hudDuration = .1f;
    [SerializeField] private CanvasGroup gameOverGroup;
    [SerializeField] private float gameOverDuration = .5f;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI hiscoreLabel;
    [SerializeField] private Button retryButton;
    [SerializeField] private string gameStateTag = "GameState";

    [Header("Events")]
    [SerializeField] private UnityEvent OnGameOver = new();

    private Score _score;
    private const string ScorePrefix = "Score: $";
    private const string HiScorePrefix = "Hi-Score: $";
    private const string HiScoreKey = "HiScore";

    private void Start()
    {
        var gameState = GameObject.FindGameObjectWithTag(gameStateTag);

        var timer = gameState.GetComponent<Timer>();
        timer.OnLose.AddListener(StartGameOverScreen);
        retryButton.onClick.AddListener(retry);

        _score = gameState.GetComponent<Score>();

        gameOverGroup.alpha = 0;
        gameOverGroup.interactable = false;
    }

    private void StartGameOverScreen()
    {
        gameOverGroup.interactable = true;

        var playerScore = _score.CurrentScore;
        scoreLabel.text = ScorePrefix + playerScore.ToString();
        SetHiScore(playerScore);

        gameOverGroup.FadeTo(1, gameOverDuration, EaseType.Linear);
        hudGroup.FadeTo(0, hudDuration, EaseType.Linear);

        OnGameOver.Invoke();
    }

    private void SetHiScore(int newScore)
    {
        if (PlayerPrefs.HasKey(HiScoreKey) == false)
        {
            PlayerPrefs.SetInt(HiScoreKey, newScore);
            hiscoreLabel.text = HiScorePrefix + newScore.ToString();
            return;
        }

        var hiScore = PlayerPrefs.GetInt(HiScoreKey);
        if (hiScore < newScore)
        {
            PlayerPrefs.SetInt(HiScoreKey, newScore);
            hiscoreLabel.text = HiScorePrefix + newScore.ToString();
            return;
        }

        hiscoreLabel.text = HiScorePrefix + hiScore.ToString();
    }

    private void retry()
    {
        retryButton.interactable = false;
        // ADD A TRANSITION
        PassengerDropoff.instances.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
