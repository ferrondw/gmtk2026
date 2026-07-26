using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject boatPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private AnimationCurve timeBetweenSpawns;
    [SerializeField] private AnimationCurve maxAllowedOnScreen;
    [SerializeField] private float secondsToMaxDifficulty;

    private float _startTime;
    private float _timeFromStart => Time.time - _startTime;

    private float _currentTimeBetweenSpawns =>
        timeBetweenSpawns.Evaluate(Mathf.Clamp01(_timeFromStart / secondsToMaxDifficulty));

    private float _currentMaxAllowedOnScreen =>
        Mathf.Round(maxAllowedOnScreen.Evaluate(Mathf.Clamp01(_timeFromStart / secondsToMaxDifficulty)));

    private List<GameObject> _currentBoats = new();

    public void StartSpawning()
    {
        StartCoroutine(nameof(PersistentSpawnBoats));
    }

    public void DestroyBoat(GameObject boat)
    {
        _currentBoats.Remove(boat);
    }

    public void SpawnBoat()
    {
        var spawnPosition = player.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        var boat = Instantiate(boatPrefab, spawnPosition, Quaternion.identity);
        _currentBoats.Add(boat);
    }

    private IEnumerator PersistentSpawnBoats()
    {
        yield return new WaitForSeconds(10);
        
        while (true)
        {
            if (_currentBoats.Count < _currentMaxAllowedOnScreen)
            {
                SpawnBoat();
                yield return new WaitForSeconds(_currentTimeBetweenSpawns);
            }
            else
            {
                yield return null;
            }
        }
    }
}