using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Score : MonoBehaviour
{
    [SerializeField] public UnityEvent<int> OnScoreUpdate = new();
    [SerializeField] public UnityEvent OnScoreChange = new();

    public int CurrentScore
    {
        get { return currentScore; }
        private set
        {
            Debug.Log("Score gone from " + currentScore.ToString() + " to " + value.ToString());

            currentScore = value;

            OnScoreUpdate.Invoke(currentScore);
            OnScoreChange.Invoke();
        }
    }

    private int currentScore;

    public void AddScore(int addedScore) => CurrentScore += addedScore;
    public void RemoveScore(int removedScore) => CurrentScore += removedScore;
}
