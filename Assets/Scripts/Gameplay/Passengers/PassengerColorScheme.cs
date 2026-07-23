using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassengerColorScheme
{
    public Color BaseColor = Color.white;
    public Color AccentColor = Color.green;

    public PassengerColorScheme(Color newColor, Color newAccentColor)
    {
        BaseColor = newColor;
        AccentColor = newAccentColor;
    }
}
