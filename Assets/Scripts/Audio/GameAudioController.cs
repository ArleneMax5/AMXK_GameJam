using UnityEngine;
using System.Collections; // ��Ҫ���� System.Collections ��ʹ��Э��

/// <summary>
/// ��Ϸ��Ƶ�����������������Ϸ״̬�������������Ų�ͬ�ı������ֺͻ�������
/// </summary>
public class GameAudioController : MonoBehaviour
{
    [Header("����������")]
    [SerializeField] private string ambientWindSound = "WindLoop"; // ��ķ�����Ƶ����

    [Header("��Ϸ����")]
    [SerializeField] private string startMusic = "GameStartMusic"; // ֻ����һ�εĿ�������
    [SerializeField] private string day1_2_Music = "Day1_2_BGM";   // ��1-2��ѭ������
    [SerializeField] private string day3_Plus_Music = "Day3_Plus_BGM"; // ��3�켰�Ժ�ѭ������

    [Header("���뵭��ʱ��")]
    [SerializeField] private float fadeTime = 2f;

    private Coroutine musicCoroutine; // ���ڸ��ٺ͹������ֲ���Э��

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager ʵ��δ�ҵ�����ȷ���������� AudioManager��");
            return;
        }

        // ��Ϸһ��ʼ���Ͳ��ų����Ļ�������
        AudioManager.Instance.PlayAmbience(ambientWindSound);

        // ���� GameManager �������仯�¼�
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDayChanged += OnDayChanged;
            // �������ݵ�ǰ������������
            OnDayChanged(GameManager.Instance.CurrentDay);
        }
        else
        {
            // ���û�� GameManager (���������˵�)�����ſ�������
            PlayMusicForDay(0); 
        }
    }

    private void OnDestroy()
    {
        // �ڶ�������ʱȡ�����ģ���ֹ�ڴ�й©
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDayChanged -= OnDayChanged;
        }
    }

    /// <summary>
    /// �������ı�ʱ�� GameManager ����
    /// </summary>
    /// <param name="newDay">�µ�һ��</param>
    private void OnDayChanged(int newDay)
    {
        PlayMusicForDay(newDay);
    }

    /// <summary>
    /// ��������������Ӧ������
    /// </summary>
    private void PlayMusicForDay(int day)
    {
        // �ڲ���������ǰ��ֹ֮ͣǰ���������е��κ�����Э��
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
        }

        if (day == 1)
        {
            // ���ڵ�һ�죬��������Ĳ�������Э��
            musicCoroutine = StartCoroutine(PlayDay1Sequence());
        }
        else if (day == 2)
        {
            Debug.Log($"�� {day} �죬��������: {day1_2_Music}");
            AudioManager.Instance.FadeInMusic(day1_2_Music, fadeTime, true); // true ��ʾѭ��
        }
        else if (day >= 3)
        {
            Debug.Log($"�� {day} �죬��������: {day3_Plus_Music}");
            AudioManager.Instance.FadeInMusic(day3_Plus_Music, fadeTime, true); // true ��ʾѭ��
        }
        else // day <= 0, ��Ӧ���˵������
        {
            Debug.Log("���ſ������� (��һ��)");
            AudioManager.Instance.FadeInMusic(startMusic, fadeTime, false); // false ��ʾ��ѭ��
        }
    }

    /// <summary>
    /// ��һ����������ֲ�������
    /// </summary>
    private IEnumerator PlayDay1Sequence()
    {
        Debug.Log("��һ�죺��ʼ���ſ������� (��һ��)");
        // 1. ���ſ������֣���ѭ��
        AudioManager.Instance.FadeInMusic(startMusic, fadeTime, false);

        // 2. ��ȷ�ȴ����ֲ������
        float startMusicLength = AudioManager.Instance.GetMusicLength(startMusic);
        
        // �����ƵƬ���Ƿ�����ҳ�����Ч
        if (startMusicLength > 0)
        {
            // �ȴ���ʱ��Ӧ������ʱ����ȥ�Ѿ��õ��ĵ���ʱ��
            // ���Ƕ�������һ����С�Ļ���ʱ�䣨0.1�룩ȷ��������ȫ����
            float waitTime = startMusicLength - fadeTime + 0.1f;
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }
        }

        // 3. ���ֲ�����Ϻ��޷��л���ѭ������
        Debug.Log("��һ�죺�������ֽ�������ʼѭ�����ų�������");
        AudioManager.Instance.FadeInMusic(day1_2_Music, fadeTime, true);
    }

    // ��Ϸ��ͣ/�ָ��Ĺ��ܿ��Ա���
    public void OnGamePaused()
    {
        AudioManager.Instance.PauseMusic();
    }

    public void OnGameResumed()
    {
        AudioManager.Instance.ResumeMusic();
    }
}