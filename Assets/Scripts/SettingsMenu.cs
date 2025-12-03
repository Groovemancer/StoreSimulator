using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown m_currencyDropdown;

    public Color m_currencyDropDownOptionsColor;

    public Slider m_mouseSensitivitySlider;
    public Toggle m_invertedCameraToggle;
    public Slider m_musicVolumeSlider;
    public Slider m_soundEffectsVolumeSlider;
    public Button m_applyChangesButton;

    private CurrencyType m_currencyType;

    private bool m_settingsDirty = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_applyChangesButton.interactable = false;
        PopulateCurrencyDropdown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateCurrencyDropdown()
    {
        m_currencyDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (string currencyName in CurrencyController.instance.GetCurrencyNames())
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = currencyName;
            optionData.color = m_currencyDropDownOptionsColor;
            options.Add(optionData);
        }

        m_currencyDropdown.AddOptions(options);
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
        m_settingsDirty = true;
        ChangesMade();
    }

    public void SetMusicVolume(float value)
    {
        Debug.Log("SettingsMenu::SetMusicVolume value=" + value);
        m_settingsDirty = true;
        ChangesMade();
    }

    public void SetSoundEffectsVolume(float value)
    {
        Debug.Log("SettingsMenu::SetSoundEffectsVolume value=" + value);
        m_settingsDirty = true;
        ChangesMade();
    }

    public void SetInvertedCamera(bool invertedCamera)
    {
        Debug.Log("SettingsMenu::SetInvertedCamera invertedCamera=" + invertedCamera);
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
        CurrencyController.instance.UpdateCurrencySetting(m_currencyType);


        // Apply to PlayerSettings
        PlayerSettings.Instance.CurrencyType = CurrencyController.instance.currentSetting.Type;

        // Save settings
        PlayerSettings.Instance.SaveSettings();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
