using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassengerColorSchemeCollection", menuName = "Boat Game/New Passenger ColorScheme Collection")]
public class PassengerColorSchemeCollection : ScriptableObject
{
    [SerializeField] private List<PassengerColorScheme> passengerColors;
    [SerializeField] private Material defaultMaterial;

    private const string MaterialBaseColor = "_SkinColor";
    private const string MaterialAccentColor = "_ClothesColor";

    private List<Material> _materials = new();

    public PassengerColorScheme GetRandomColorScheme()
    {
        var number = Random.Range(0, passengerColors.Count);
        return passengerColors[number];
    }

    public void CreateMaterials()
    {
        if (_materials.Count == passengerColors.Count && _materials[0] != null)
        {
            Debug.LogWarning("Attempting to make new materials, aborting!");
            return;
        }

        foreach (var material in _materials) DestroyImmediate(material);
        _materials.Clear();

        foreach (var colorScheme in passengerColors)
        {
            var newMaterial = new Material(defaultMaterial);

            newMaterial.EnableKeyword(MaterialBaseColor);
            newMaterial.EnableKeyword(MaterialAccentColor);

            newMaterial.SetColor(MaterialBaseColor, colorScheme.BaseColor);
            newMaterial.SetColor(MaterialAccentColor, colorScheme.AccentColor);

            _materials.Add(newMaterial);

            Debug.Log("Created material with base color " +  colorScheme.BaseColor.ToString() + " and accent color " + colorScheme.AccentColor.ToString());
        }
    }

    public Material GetColorSchemeMaterial(PassengerColorScheme checkColorScheme)
    {
        if (_materials.Count != passengerColors.Count)
        {
            Debug.LogError("Colorschemes and static passenger materials mismatch!");
            return null;
        }

        for (var index = 0; index < passengerColors.Count; index++) if (passengerColors[index] == checkColorScheme) return _materials[index];

        Debug.LogError("Colorschemes doesnt have an associated material!");
        return null;
    }
}
