using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterStat Health;
    public CharacterStat Power;

    void Start()
    {
        Health = new CharacterStat(100);
        Power = new CharacterStat(100);
    }

    void OnGUI()
    {
        GUILayout.Label("Health: " + Health.Value);
        GUILayout.Label("Power: " + Power.Value);
    }
}
