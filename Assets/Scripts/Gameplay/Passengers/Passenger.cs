using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Passenger
{
    public string DropoffId;
    public PassengerColorScheme ColorScheme;

    public int MoodStates = 6;
    public int Score = 100;
    public int Time = 12;
    public int Risk = 0;

    public Passenger(string newId, PassengerColorScheme newColorScheme, int newMood = 6, int newScore = 100, int newTime = 12, int newRisk = 0)
    {
        DropoffId = newId;
        ColorScheme = newColorScheme;
        MoodStates = newMood;
        Score = newScore;
        Time = newTime;
        Risk = newRisk;
    }
}
