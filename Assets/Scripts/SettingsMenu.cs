using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown m_currencyDropdown;
    public Slider m_mouseSensitivitySlider;
    public Toggle m_invertedCameraToggle;
    public Slider m_musicVolumeSlider;
    public Slider m_soundEffectsVolumeSlider;
    public Button m_applyChangesButton;

    private bool m_settingsDirty = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_applyChangesButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrency(int  currencyValue)
    {
        Debug.Log("SettingsMenu::SetCurrency currencyValue=" + currencyValue);
        m_settingsDirty = true;
    }

    public void SetMouseSensitivity(float value)
    {
        Debug.Log("SettingsMenu::SetMouseSensitivity value=" + value);
        m_settingsDirty = true;
    }

    public void SetMusicVolume(float value)
    {
        Debug.Log("SettingsMenu::SetMusicVolume value=" + value);
        m_settingsDirty = true;
    }

    public void SetSoundEffectsVolume(float value)
    {
        Debug.Log("SettingsMenu::SetSoundEffectsVolume value=" + value);
        m_settingsDirty = true;
    }

    public void SetInvertedCamera(bool invertedCamera)
    {
        Debug.Log("SettingsMenu::SetInvertedCamera invertedCamera=" + invertedCamera);
        m_settingsDirty = true;
    }

    public void ApplyChanges()
    {
        Debug.Log("SettingsMenu::ApplyChanges");
        m_settingsDirty = false;
    }
}
