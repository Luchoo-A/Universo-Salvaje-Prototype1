using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShieldStats", menuName = "Shield/Stats")]
public class ShieldStatsSO : ScriptableObject
{
    [Header("Identificación")]

    public string ShieldName = "Nuevo Escudo";


    [Header("Stats Basicos")]
    public float BaseShieldPoints = 50;

    public float timeToActive = 10f;


}