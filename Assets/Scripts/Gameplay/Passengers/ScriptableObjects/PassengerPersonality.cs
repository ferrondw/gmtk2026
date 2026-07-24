using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassengerPersonality
{
    [SerializeField] public Color ZoneColor;
    [SerializeField] public int MoodStates = 6;
    [SerializeField] public int Score = 100;
    [SerializeField] public int Time = 12;
    [SerializeField][Range(0, 10)] public int PoliceRisk = 0; // ADD POLICE RISK MECHANIC
}
