using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown m_currencyDropdown;

    public Color m_currencyDropDownOptionsColor;

    public Slider m_mouseSensitivitySlider;
    public Slider m_controllerSensitivitySlider;
    public Toggle m_invertedCameraToggle;
    public Slider m_musicVolumeSlider;
    public Slider m_soundEffectsVolumeSlider;
    public Button m_applyChangesButton;

    private CurrencyType m_currencyType = CurrencyType.USD;
    private float m_mouseSensitivity = 0.5f;
    private float m_controllerSensitivity = 0.5f;
    private bool m_invertedCamera = false;
    private float m_musicVolume = 0.5f;
    private float m_soundEffectsVolume = 0.5f;

    private bool m_settingsDirty = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_applyChangesButton.interactable = false;
        PopulateCurrencyDropdown();
        LoadInitialSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateCurrencyDropdown()
    {
        m_currencyDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (string currencyName in CurrencyManager.Instance.GetCurrencyNames())
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = currencyName;
            optionData.color = m_currencyDropDownOptionsColor;
            options.Add(optionData);
        }

        m_currencyDropdown.AddOptions(options);
    }

    private void LoadInitialSettings()
    {
        m_currencyType = PlayerConfigSettings.Instance.CurrencyType;
        m_mouseSensitivity = PlayerConfigSettings.Instance.MouseSensitivity;
        m_controllerSensitivity = PlayerConfigSettings.Instance.ControllerSensitivity;
        m_invertedCamera = PlayerConfigSettings.Instance.InvertedCamera;

        m_musicVolume = PlayerConfigSettings.Instance.MusicVolume;
        m_soundEffectsVolume = PlayerConfigSettings.Instance.SoundVolume;

        m_currencyDropdown.value = (int)m_currencyType;
        m_mouseSensitivitySlider.value = m_mouseSensitivity;
        m_controllerSensitivitySlider.value = m_controllerSensitivity;
        m_invertedCameraToggle.isOn = m_invertedCamera;

        m_musicVolumeSlider.value = m_musicVolume;
        m_soundEffectsVolumeSlider.value = m_soundEffectsVolume;
    }

    public void SetCurrency(int  currencyValue)
    {
        Debug.Log("SettingsMenu::SetCurrency currencyValue=" + currencyValue);

        m_currencyType = (CurrencyType)currencyValue;

        m_settingsDirty = true;
        ChangesMade();
    }

    public void SetMouseSensitivity(float value)
    {
        Debug.Log("SettingsMenu::SetMouseSensitivity value=" + value);
        m_mouseSensitivity = value;
        m_settingsDirty = true;
        ChangesMade();
    }

    public void SetControllerSensitivity(float value)
    {
        Debug.Log("SettingsMenu::SetControllerSensitivity value=" + value);
        m_controllerSensitivity = value;
        m_settingsDirty = true;
        ChangesMade();
    }

    public void SetMusicVolume(float value)
    {
        Debug.Log("SettingsMenu::SetMusicVolume value=" + value);
        m_musicVolume = value;
        m_settingsDirty = true;
        ChangesMade();
    }

    public void SetSoundEffectsVolume(float value)
    {
        Debug.Log("SettingsMenu::SetSoundEffectsVolume value=" + value);
        m_soundEffectsVolume = value;
        m_settingsDirty = true;
        ChangesMade();
    }

    public void SetInvertedCamera(bool invertedCamera)
    {
        Debug.Log("SettingsMenu::SetInvertedCamera invertedCamera=" + invertedCamera);
        m_invertedCamera = invertedCamera;
        m_settingsDirty = true;
        ChangesMade();
    }

    public void ChangesMade()
    {
        m_applyChangesButton.interactable = true;
    }

    public void ApplyChanges()
    {
        Debug.Log("SettingsMenu::ApplyChanges");
        m_settingsDirty = false;
        m_applyChangesButton.interactable = false;

        // Update the systems
        CurrencyManager.Instance.UpdateCurrencySetting(m_currencyType);

        AudioManager.instance.SetMusicVolume(m_musicVolume);
        AudioManager.instance.SetSoundEffectsVolume(m_soundEffectsVolume);


        // Apply to PlayerSettings
        PlayerConfigSettings.Instance.CurrencyType = CurrencyManager.Instance.currentSetting.Type;
        PlayerConfigSettings.Instance.MouseSensitivity = m_mouseSensitivity;
        PlayerConfigSettings.Instance.ControllerSensitivity = m_controllerSensitivity;
        PlayerConfigSettings.Instance.InvertedCamera = m_invertedCamera;

        PlayerConfigSettings.Instance.MusicVolume = m_musicVolume;
        PlayerConfigSettings.Instance.SoundVolume = m_soundEffectsVolume;


        // Save settings
        PlayerConfigSettings.Instance.SaveSettings();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
