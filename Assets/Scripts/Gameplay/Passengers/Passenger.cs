using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Passenger
{
    public string DropoffId;
    public PassengerColorScheme ColorScheme;

    public Passenger(string newId, PassengerColorScheme newColorScheme)
    {
        DropoffId = newId;
        ColorScheme = newColorScheme;
    }
}
