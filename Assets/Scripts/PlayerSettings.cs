using System;
using UnityEngine;

public class PlayerSettings
{
    private static PlayerSettings _instance;
    public static PlayerSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerSettings();
            }
            return _instance;
        }
    }

    public float MouseSensitivity;
    public float MusicVolume;
    public float SoundVolume;
    public bool InvertedCamera;
    public int CurrencyType;

    public void LoadSettings()
    {
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        SoundVolume = PlayerPrefs.GetFloat("SoundVolume");
        InvertedCamera = (PlayerPrefs.GetInt("InvertedCamera") != 0);
        CurrencyType = PlayerPrefs.GetInt("CurrencyType");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", MouseSensitivity);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
        PlayerPrefs.SetInt("InvertedCamera", Convert.ToInt32(InvertedCamera));
        PlayerPrefs.SetInt("CurrencyType", CurrencyType);
        PlayerPrefs.Save();
    }
}
