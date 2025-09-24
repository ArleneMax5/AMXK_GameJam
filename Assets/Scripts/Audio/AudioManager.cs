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

    [Header("音频源")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambienceSource; // 新增：环境音源

    [Header("背景音乐")]
    [SerializeField] private List<SoundClip> backgroundMusic = new List<SoundClip>();

    [Header("音效")]
    [SerializeField] private List<SoundClip> soundEffects = new List<SoundClip>();

    [Header("环境音 (循环)")] // 新增
    [SerializeField] private List<SoundClip> ambientSounds = new List<SoundClip>();

    [Header("音量设置")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float ambienceVolume = 0.5f; // 新增：环境音量

    private Dictionary<string, SoundClip> musicDict = new Dictionary<string, SoundClip>();
    private Dictionary<string, SoundClip> sfxDict = new Dictionary<string, SoundClip>();
    private Dictionary<string, SoundClip> ambienceDict = new Dictionary<string, SoundClip>(); // 新增

    private void Awake()
    {
        // 单例模式
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
        // 创建音频源如果不存在
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

        // 新增：创建环境音源
        if (ambienceSource == null)
        {
            GameObject ambienceGO = new GameObject("Ambience Source");
            ambienceGO.transform.SetParent(transform);
            ambienceSource = ambienceGO.AddComponent<AudioSource>();
            ambienceSource.loop = true;
            ambienceSource.playOnAwake = false;
        }

        // 初始化字典
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
        
        // 新增：初始化环境音字典
        foreach (var ambience in ambientSounds)
        {
            if (!string.IsNullOrEmpty(ambience.name) && !ambienceDict.ContainsKey(ambience.name))
            {
                ambienceDict.Add(ambience.name, ambience);
            }
        }

        // 应用音量设置
        UpdateVolumes();
    }

    #region 环境音控制 (新增)
    public void PlayAmbience(string ambienceName)
    {
        if (ambienceDict.TryGetValue(ambienceName, out SoundClip ambience))
        {
            if (ambienceSource.clip == ambience.clip && ambienceSource.isPlaying)
            {
                return; // 如果已经在播放同一个环境音，则不打断
            }

            ambienceSource.clip = ambience.clip;
            ambienceSource.volume = ambience.volume * ambienceVolume * masterVolume;
            ambienceSource.pitch = ambience.pitch;
            ambienceSource.loop = ambience.loop; // 环境音通常是循环的
            ambienceSource.Play();
        }
        else
        {
            Debug.LogWarning($"环境音 '{ambienceName}' 未找到！");
        }
    }

    public void StopAmbience()
    {
        ambienceSource.Stop();
    }
    #endregion

    #region 背景音乐控制
    public void PlayMusic(string musicName, bool loop = true) // 增加循环参数
    {
        if (musicDict.TryGetValue(musicName, out SoundClip music))
        {
            musicSource.clip = music.clip;
            musicSource.volume = music.volume * musicVolume * masterVolume;
            musicSource.pitch = music.pitch;
            musicSource.loop = loop; // 使用传入的参数
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"背景音乐 '{musicName}' 未找到！");
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

    public void FadeInMusic(string musicName, float duration = 1f, bool loop = true) // 增加循环参数
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

    private IEnumerator FadeInMusicCoroutine(SoundClip music, float duration, bool loop) // 增加循环参数
    {
        musicSource.clip = music.clip;
        musicSource.volume = 0f;
        musicSource.pitch = music.pitch;
        musicSource.loop = loop; // 使用传入的参数
        musicSource.Play();

        float targetVolume = music.volume * musicVolume * masterVolume;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime; // 使用 unscaledDeltaTime 以免受 timeScale 影响
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
            currentTime += Time.unscaledDeltaTime; // 使用 unscaledDeltaTime
            musicSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }

    // 新增：获取音乐片段的时长
    public float GetMusicLength(string musicName)
    {
        if (musicDict.TryGetValue(musicName, out SoundClip music))
        {
            if (music.clip != null)
            {
                return music.clip.length;
            }
        }
        Debug.LogWarning($"无法获取音乐 '{musicName}' 的时长，请检查配置。");
        return 0f;
    }
    #endregion

    #region 音效控制
    public void PlaySFX(string sfxName)
    {
        if (sfxDict.TryGetValue(sfxName, out SoundClip sfx))
        {
            sfxSource.pitch = sfx.pitch;
            sfxSource.PlayOneShot(sfx.clip, sfx.volume * sfxVolume * masterVolume);
        }
        else
        {
            Debug.LogWarning($"音效 '{sfxName}' 未找到！");
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

    #region 音量控制
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

    public void SetAmbienceVolume(float volume) // 新增
    {
        ambienceVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        // 更新音乐音量
        if (musicSource.isPlaying)
        {
            if (musicDict.TryGetValue(musicSource.clip.name, out var music))
            {
                musicSource.volume = music.volume * musicVolume * masterVolume;
            }
        }
        // 新增：更新环境音音量
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
    public float GetAmbienceVolume() => ambienceVolume; // 新增
    #endregion
}