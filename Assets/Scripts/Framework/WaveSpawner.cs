using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private AnimationCurve waveDifficulty;
    [SerializeField] private float secondsToMaxDifficulty;

    private float _startTime;
    
    
}