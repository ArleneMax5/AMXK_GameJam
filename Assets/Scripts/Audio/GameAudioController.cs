using UnityEngine;
using System.Collections; // 需要引入 System.Collections 以使用协程

/// <summary>
/// 游戏音频控制器，负责根据游戏状态（如天数）播放不同的背景音乐和环境音。
/// </summary>
public class GameAudioController : MonoBehaviour
{
    [Header("持续环境音")]
    [SerializeField] private string ambientWindSound = "WindLoop"; // 你的风声音频名称

    [Header("游戏音乐")]
    [SerializeField] private string startMusic = "GameStartMusic"; // 只播放一次的开场音乐
    [SerializeField] private string day1_2_Music = "Day1_2_BGM";   // 第1-2天循环音乐
    [SerializeField] private string day3_Plus_Music = "Day3_Plus_BGM"; // 第3天及以后循环音乐

    [Header("淡入淡出时间")]
    [SerializeField] private float fadeTime = 2f;

    private Coroutine musicCoroutine; // 用于跟踪和管理音乐播放协程

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager 实例未找到！请确保场景中有 AudioManager。");
            return;
        }

        // 游戏一开始，就播放持续的环境风声
        AudioManager.Instance.PlayAmbience(ambientWindSound);

        // 订阅 GameManager 的天数变化事件
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDayChanged += OnDayChanged;
            // 立即根据当前天数播放音乐
            OnDayChanged(GameManager.Instance.CurrentDay);
        }
        else
        {
            // 如果没有 GameManager (可能在主菜单)，播放开场音乐
            PlayMusicForDay(0); 
        }
    }

    private void OnDestroy()
    {
        // 在对象销毁时取消订阅，防止内存泄漏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDayChanged -= OnDayChanged;
        }
    }

    /// <summary>
    /// 当天数改变时由 GameManager 调用
    /// </summary>
    /// <param name="newDay">新的一天</param>
    private void OnDayChanged(int newDay)
    {
        PlayMusicForDay(newDay);
    }

    /// <summary>
    /// 根据天数播放相应的音乐
    /// </summary>
    private void PlayMusicForDay(int day)
    {
        // 在播放新音乐前，停止之前可能在运行的任何音乐协程
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
        }

        if (day == 1)
        {
            // 对于第一天，启动特殊的播放序列协程
            musicCoroutine = StartCoroutine(PlayDay1Sequence());
        }
        else if (day == 2)
        {
            Debug.Log($"第 {day} 天，播放音乐: {day1_2_Music}");
            AudioManager.Instance.FadeInMusic(day1_2_Music, fadeTime, true); // true 表示循环
        }
        else if (day >= 3)
        {
            Debug.Log($"第 {day} 天，播放音乐: {day3_Plus_Music}");
            AudioManager.Instance.FadeInMusic(day3_Plus_Music, fadeTime, true); // true 表示循环
        }
        else // day <= 0, 对应主菜单等情况
        {
            Debug.Log("播放开场音乐 (仅一次)");
            AudioManager.Instance.FadeInMusic(startMusic, fadeTime, false); // false 表示不循环
        }
    }

    /// <summary>
    /// 第一天的特殊音乐播放序列
    /// </summary>
    private IEnumerator PlayDay1Sequence()
    {
        Debug.Log("第一天：开始播放开场音乐 (仅一次)");
        // 1. 播放开场音乐，不循环
        AudioManager.Instance.FadeInMusic(startMusic, fadeTime, false);

        // 2. 精确等待音乐播放完毕
        float startMusicLength = AudioManager.Instance.GetMusicLength(startMusic);
        
        // 检查音频片段是否存在且长度有效
        if (startMusicLength > 0)
        {
            // 等待的时间应该是总时长减去已经用掉的淡入时间
            // 我们额外增加一个很小的缓冲时间（0.1秒）确保淡入完全结束
            float waitTime = startMusicLength - fadeTime + 0.1f;
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }
        }

        // 3. 音乐播放完毕后，无缝切换到循环音乐
        Debug.Log("第一天：开场音乐结束，开始循环播放常规音乐");
        AudioManager.Instance.FadeInMusic(day1_2_Music, fadeTime, true);
    }

    // 游戏暂停/恢复的功能可以保留
    public void OnGamePaused()
    {
        AudioManager.Instance.PauseMusic();
    }

    public void OnGameResumed()
    {
        AudioManager.Instance.ResumeMusic();
    }
}