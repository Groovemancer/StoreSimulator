using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource titleMusic;

    public List<AudioSource> bgm = new List<AudioSource>();

    public List<AudioSource> sfx = new List<AudioSource>();

    private bool bgmPlaying;
    private int currentTrack;

    private float m_musicVolume = 0.5f;
    private float m_soundEffectsVolume = 0.5f;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_musicVolume = PlayerConfigSettings.Instance.MusicVolume;
        m_soundEffectsVolume = PlayerConfigSettings.Instance.SoundVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (bgmPlaying == true)
        {
            if (bgm[currentTrack].isPlaying == false)
            {
                StartBGM();
            }
        }
    }

    public void StopMusic()
    {
        titleMusic.Stop();
        foreach (AudioSource track in bgm)
        {
            track.Stop();
        }
        bgmPlaying = false;
    }

    public void StartTitleMusic()
    {
        StopMusic();
        titleMusic.volume = m_musicVolume;
        titleMusic.Play();
    }

    public void StartBGM()
    {
        StopMusic();
        bgmPlaying = true;
        currentTrack = Random.Range(0, bgm.Count);
        bgm[currentTrack].volume = m_musicVolume;
        bgm[currentTrack].Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].volume = m_soundEffectsVolume;
        sfx[sfxToPlay].Play();
    }

    public void SetMusicVolume(float volume)
    {
        m_musicVolume = volume;
        titleMusic.volume = m_musicVolume;
        foreach (AudioSource track in bgm)
        {
            track.volume = m_musicVolume;
        }
    }

    public void SetSoundEffectsVolume(float volume)
    {
        m_soundEffectsVolume = volume;
    }
}
