using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundClip
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool loop = false;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("��ƵԴ")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambienceSource; // ������������Դ

    [Header("��������")]
    [SerializeField] private List<SoundClip> backgroundMusic = new List<SoundClip>();

    [Header("��Ч")]
    [SerializeField] private List<SoundClip> soundEffects = new List<SoundClip>();

    [Header("������ (ѭ��)")] // ����
    [SerializeField] private List<SoundClip> ambientSounds = new List<SoundClip>();

    [Header("��������")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float ambienceVolume = 0.5f; // ��������������

    private Dictionary<string, SoundClip> musicDict = new Dictionary<string, SoundClip>();
    private Dictionary<string, SoundClip> sfxDict = new Dictionary<string, SoundClip>();
    private Dictionary<string, SoundClip> ambienceDict = new Dictionary<string, SoundClip>(); // ����

    private void Awake()
    {
        // ����ģʽ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudio()
    {
        // ������ƵԴ���������
        if (musicSource == null)
        {
            GameObject musicGO = new GameObject("Music Source");
            musicGO.transform.SetParent(transform);
            musicSource = musicGO.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            GameObject sfxGO = new GameObject("SFX Source");
            sfxGO.transform.SetParent(transform);
            sfxSource = sfxGO.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        // ����������������Դ
        if (ambienceSource == null)
        {
            GameObject ambienceGO = new GameObject("Ambience Source");
            ambienceGO.transform.SetParent(transform);
            ambienceSource = ambienceGO.AddComponent<AudioSource>();
            ambienceSource.loop = true;
            ambienceSource.playOnAwake = false;
        }

        // ��ʼ���ֵ�
        foreach (var music in backgroundMusic)
        {
            if (!string.IsNullOrEmpty(music.name) && !musicDict.ContainsKey(music.name))
            {
                musicDict.Add(music.name, music);
            }
        }

        foreach (var sfx in soundEffects)
        {
            if (!string.IsNullOrEmpty(sfx.name) && !sfxDict.ContainsKey(sfx.name))
            {
                sfxDict.Add(sfx.name, sfx);
            }
        }
        
        // ��������ʼ���������ֵ�
        foreach (var ambience in ambientSounds)
        {
            if (!string.IsNullOrEmpty(ambience.name) && !ambienceDict.ContainsKey(ambience.name))
            {
                ambienceDict.Add(ambience.name, ambience);
            }
        }

        // Ӧ����������
        UpdateVolumes();
    }

    #region ���������� (����)
    public void PlayAmbience(string ambienceName)
    {
        if (ambienceDict.TryGetValue(ambienceName, out SoundClip ambience))
        {
            if (ambienceSource.clip == ambience.clip && ambienceSource.isPlaying)
            {
                return; // ����Ѿ��ڲ���ͬһ�����������򲻴��
            }

            ambienceSource.clip = ambience.clip;
            ambienceSource.volume = ambience.volume * ambienceVolume * masterVolume;
            ambienceSource.pitch = ambience.pitch;
            ambienceSource.loop = ambience.loop; // ������ͨ����ѭ����
            ambienceSource.Play();
        }
        else
        {
            Debug.LogWarning($"������ '{ambienceName}' δ�ҵ���");
        }
    }

    public void StopAmbience()
    {
        ambienceSource.Stop();
    }
    #endregion

    #region �������ֿ���
    public void PlayMusic(string musicName, bool loop = true) // ����ѭ������
    {
        if (musicDict.TryGetValue(musicName, out SoundClip music))
        {
            musicSource.clip = music.clip;
            musicSource.volume = music.volume * musicVolume * masterVolume;
            musicSource.pitch = music.pitch;
            musicSource.loop = loop; // ʹ�ô���Ĳ���
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"�������� '{musicName}' δ�ҵ���");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void FadeInMusic(string musicName, float duration = 1f, bool loop = true) // ����ѭ������
    {
        if (musicDict.TryGetValue(musicName, out SoundClip music))
        {
            StartCoroutine(FadeInMusicCoroutine(music, duration, loop));
        }
    }

    public void FadeOutMusic(float duration = 1f)
    {
        StartCoroutine(FadeOutMusicCoroutine(duration));
    }

    private IEnumerator FadeInMusicCoroutine(SoundClip music, float duration, bool loop) // ����ѭ������
    {
        musicSource.clip = music.clip;
        musicSource.volume = 0f;
        musicSource.pitch = music.pitch;
        musicSource.loop = loop; // ʹ�ô���Ĳ���
        musicSource.Play();

        float targetVolume = music.volume * musicVolume * masterVolume;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime; // ʹ�� unscaledDeltaTime ������ timeScale Ӱ��
            musicSource.volume = Mathf.Lerp(0f, targetVolume, currentTime / duration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    private IEnumerator FadeOutMusicCoroutine(float duration)
    {
        float startVolume = musicSource.volume;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime; // ʹ�� unscaledDeltaTime
            musicSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }

    // ��������ȡ����Ƭ�ε�ʱ��
    public float GetMusicLength(string musicName)
    {
        if (musicDict.TryGetValue(musicName, out SoundClip music))
        {
            if (music.clip != null)
            {
                return music.clip.length;
            }
        }
        Debug.LogWarning($"�޷���ȡ���� '{musicName}' ��ʱ�����������á�");
        return 0f;
    }
    #endregion

    #region ��Ч����
    public void PlaySFX(string sfxName)
    {
        if (sfxDict.TryGetValue(sfxName, out SoundClip sfx))
        {
            sfxSource.pitch = sfx.pitch;
            sfxSource.PlayOneShot(sfx.clip, sfx.volume * sfxVolume * masterVolume);
        }
        else
        {
            Debug.LogWarning($"��Ч '{sfxName}' δ�ҵ���");
        }
    }

    public void PlaySFXAtPosition(string sfxName, Vector3 position)
    {
        if (sfxDict.TryGetValue(sfxName, out SoundClip sfx))
        {
            AudioSource.PlayClipAtPoint(sfx.clip, position, sfx.volume * sfxVolume * masterVolume);
        }
    }
    #endregion

    #region ��������
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetAmbienceVolume(float volume) // ����
    {
        ambienceVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        // ������������
        if (musicSource.isPlaying)
        {
            if (musicDict.TryGetValue(musicSource.clip.name, out var music))
            {
                musicSource.volume = music.volume * musicVolume * masterVolume;
            }
        }
        // ���������»���������
        if (ambienceSource.isPlaying)
        {
            if (ambienceDict.TryGetValue(ambienceSource.clip.name, out var ambience))
            {
                ambienceSource.volume = ambience.volume * ambienceVolume * masterVolume;
            }
        }
    }

    public float GetMasterVolume() => masterVolume;
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
    public float GetAmbienceVolume() => ambienceVolume; // ����
    #endregion
}