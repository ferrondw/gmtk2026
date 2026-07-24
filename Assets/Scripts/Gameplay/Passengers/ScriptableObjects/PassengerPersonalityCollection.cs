using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassengerPersonaCollection", menuName = "Boat Game/New Passenger Persona Collection")]
public class PassengerPersonalityCollection : ScriptableObject
{
    [SerializeField] private List<PassengerPersonality> personalities = new();

    public PassengerPersonality GetRandomPersona()
    {
        var number = Random.Range(0, personalities.Count);
        return personalities[number];
    }
}
