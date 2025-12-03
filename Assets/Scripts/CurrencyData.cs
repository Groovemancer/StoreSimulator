using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public enum CurrencyType
{
    USD, EUR, JPY, GBP, CAD, AUD, CNY_RMB
}

[Serializable]
public class CurrencySetting
{
    public string Name;
    public CurrencyType Type;    
    public string Symbol;
    public float ExchangeRate = 1.0f; // From USD to currency
}

[CreateAssetMenu(fileName = "CurrencyData", menuName = "Scriptable Objects/CurrencyData")]
public class CurrencyData : ScriptableObject 
{
    public List<CurrencySetting> Currencies;
}