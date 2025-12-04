using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField]
    private CurrencyData m_currencyData;

    public PlayerInput playerInput;

    private bool m_loadData = true;

    private void Awake()
    {
        EnableDontDestroyOnLoad();

        if (m_loadData)
        {
            LoadData();
        }
    }

    private void Start()
    {
        AudioManager.instance.SetMusicVolume(PlayerConfigSettings.Instance.MusicVolume);
        AudioManager.instance.SetSoundEffectsVolume(PlayerConfigSettings.Instance.SoundVolume);
    }

    private void LoadData()
    {
        CurrencyManager.Instance.Initialize(m_currencyData);

        PlayerConfigSettings.Instance.LoadSettings();

        CurrencyManager.Instance.UpdateCurrencySetting(PlayerConfigSettings.Instance.CurrencyType);
    }

    private void EnableDontDestroyOnLoad()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            m_loadData = true;
        }
        else
        {
            Destroy(gameObject);

            m_loadData = false;
        }
    }
}