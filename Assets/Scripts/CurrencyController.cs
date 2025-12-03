using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    public static CurrencyController instance;

    [SerializeField]
    private CurrencyData m_currencyData;

    public CurrencySetting currentSetting;

    public Action<CurrencyType> OnCurrencyChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentSetting = m_currencyData.Currencies.Find(cs => cs.Type == CurrencyType.USD); // default
    }

    public CurrencyData GetCurrencyData()
    {
        return m_currencyData;
    }

    public void UpdateCurrencySetting(CurrencyType newCurrencyType)
    {
        CurrencyType oldCurrencyType = currentSetting.Type;
        currentSetting = m_currencyData.Currencies.Find(cs => cs.Type == newCurrencyType);

        if (OnCurrencyChanged != null)
        {
            OnCurrencyChanged(oldCurrencyType);
        }
    }

    public float ConvertMoney(float initialAmount)
    {
        return initialAmount * currentSetting.ExchangeRate;
    }

    public float ConvertToDollars(float amount)
    {
        return amount / currentSetting.ExchangeRate;
    }

    public string GetCurrencySymbol()
    {
        return currentSetting.Symbol;
    }

    public List<string> GetCurrencyNames()
    {
        List<string> currencyNames = new List<string>();
        foreach (CurrencySetting currencySetting in m_currencyData.Currencies)
        {
            currencyNames.Add(currencySetting.Name);
        }
        
        return currencyNames;
    }
}
