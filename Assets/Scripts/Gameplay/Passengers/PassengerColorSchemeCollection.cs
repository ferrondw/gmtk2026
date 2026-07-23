using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[CreateAssetMenu(fileName = "NewPassengerColorSchemeCollection", menuName = "Boat Game/New Passenger ColorScheme Collection")]
public class PassengerColorSchemeCollection : ScriptableObject
{
    [SerializeField] private List<PassengerColorScheme> passengerColors;

    public PassengerColorScheme GetRandomColorScheme()
    {
        var number = Random.Range(0, passengerColors.Count);
        return passengerColors[number];
    }
}
