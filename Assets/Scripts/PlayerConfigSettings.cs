using System;
using UnityEngine;

public class PlayerConfigSettings
{
    private static PlayerConfigSettings _instance;
    public static PlayerConfigSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerConfigSettings();
            }
            return _instance;
        }
    }

    public float MouseSensitivity;
    public float ControllerSensitivity;
    public float MusicVolume;
    public float SoundVolume;
    public bool InvertedCamera;
    public CurrencyType CurrencyType;

    public void LoadSettings()
    {
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        ControllerSensitivity = PlayerPrefs.GetFloat("ControllerSensitivity", 0.5f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        InvertedCamera = (PlayerPrefs.GetInt("InvertedCamera", 0) != 0);
        CurrencyType = (CurrencyType)PlayerPrefs.GetInt("CurrencyType", 0);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", MouseSensitivity);
        PlayerPrefs.SetFloat("ControllerSensitivity", ControllerSensitivity);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
        PlayerPrefs.SetInt("InvertedCamera", Convert.ToInt32(InvertedCamera));
        PlayerPrefs.SetInt("CurrencyType", (int)CurrencyType);
        PlayerPrefs.Save();
    }
}
