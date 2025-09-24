using System;
using UnityEngine;

// ������Ϸ�ĺ������ݺ��߼�
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("�������")]
    [SerializeField] private int dailyActionPoints = 10;
    [SerializeField] private int dailyHungerLoss = 5;
    [SerializeField] private int dailySanityLoss = 5;
    [SerializeField] private int minHunger = 0;
    [SerializeField] private int minSanity = 0;

    // --- ������Ϸ���ݣ��ⲿֻ���� ---
    public int CurrentDay { get; private set; }
    public int ActionPoints { get; private set; }
    public int Health { get; private set; }
    public int Hunger { get; private set; }
    public int Sanity { get; private set; }
    public int Food { get; private set; }
    public int Collectibles { get; private set; }
    public int Medicine { get; private set; }

    // ����ж��㣨�� UI ��ȡ��
    public int MaxActionPoints => dailyActionPoints;

    // --- ��Ϸ״̬ ---
    public bool IsGameOver { get; private set; }

    // --- �¼� ---
    public event Action OnStatsChanged;
    public event Action<int> OnDayChanged; // �����������仯�¼�

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void RaiseStatsChanged() => OnStatsChanged?.Invoke();

    private void CheckGameOver()
    {
        if (!IsGameOver)
        {
            if (Health <= 0 || Hunger <= 0 || Sanity <= 0)
            {
                IsGameOver = true;
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.PushPanel(PanelType.GameOver);
                }
            }
        }
    }

    // �� GameOverPanel ����
    public void RestartGame()
    {
        IsGameOver = false;
        InitializeNewGame();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ClearAndPushPanel(PanelType.MainMenu);
        }
    }

    // ��ʼ������Ϸ
    public void InitializeNewGame()
    {
        CurrentDay = 1;
        ActionPoints = dailyActionPoints; // �������� dailyActionPoints ����
        Health = 100;
        Hunger = 100;
        Sanity = 100;
        Food = 0;
        Collectibles = 0;
        Medicine = 0;
        IsGameOver = false;

        RaiseStatsChanged();
        // ����ʼ����Ϸʱ��ҲӦ�ô��������仯�¼�����ȷ��������ȷ����
        OnDayChanged?.Invoke(CurrentDay);
    }

    public void FastForwardDay()
    {
        if (IsGameOver) return;

        CurrentDay++;
        ActionPoints = dailyActionPoints;
        
        // --- �޸ĵ� ---
        // ʹ�����л��ı��������ټ����;���ֵ
        Hunger = Mathf.Max(minHunger, Hunger - dailyHungerLoss);
        Sanity = Mathf.Max(minSanity, Sanity - dailySanityLoss);
        // ------------

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("FastForward");
        }

        RaiseStatsChanged();
        
        // �������ı���������� OnDayChanged �¼��������µ��������ݳ�ȥ
        OnDayChanged?.Invoke(CurrentDay);

        CheckGameOver();
    }

    // �����ж��㣨����ť���߼����ã�
    public bool TrySpendActionPoint(int cost = 1)
    {
        if (IsGameOver) return false;
        if (cost <= 0) return true;
        if (ActionPoints < cost) return false;

        ActionPoints -= cost;
        RaiseStatsChanged();
        return true;
    }

    public void ApplySaveData(
        int health, int hunger, int sanity,
        int currentDay, int food, int collectibles, int medicine, int actionPoints)
    {
        Health = health;
        Hunger = hunger;
        Sanity = sanity;
        CurrentDay = currentDay;
        Food = food;
        Collectibles = collectibles;
        Medicine = medicine;
        ActionPoints = actionPoints;
        IsGameOver = false;

        RaiseStatsChanged();
        // �Ӵ浵����ʱ��ҲӦ�ô��������仯�¼�
        OnDayChanged?.Invoke(CurrentDay);
        CheckGameOver();
    }   

    /// <summary>
    /// ʹ��ҩƷ���ָ�20������ֵ
    /// </summary>
    public void UseMedicine()
    {
        if (Medicine > 0)
        {
            Medicine--;
            Health = Mathf.Min(Health + 20, 100); // �������ֵΪ100
            RaiseStatsChanged();
        }
    }

    /// <summary>
    /// ʹ��ʳ��ָ�10�㼢��ֵ
    /// </summary>
    public void UseFood()
    {
        if (Food > 0)
        {
            Food--;
            Hunger = Mathf.Min(Hunger + 10, 100); // �������ֵΪ100
            RaiseStatsChanged();
        }
    }

    /// <summary>
    /// ʹ���ղ�Ʒ���ָ�10�㾫��ֵ
    /// </summary>
    public void UseCollectible()
    {
        if (Collectibles > 0)
        {
            Collectibles--;
            Sanity = Mathf.Min(Sanity + 10, 100); // �������ֵΪ100
            RaiseStatsChanged();
        }
    }

    // ==== �����ָ���ڣ��� SaveLoadManager ���ã� ====
    public void ApplySaveData(
        int health, int hunger, int sanity,
        int currentDay, int food, int collectibles, int medicine, int actionPoints)
    {
        Health = health;
        Hunger = hunger;
        Sanity = sanity;

        CurrentDay = currentDay;
        Food = food;
        Collectibles = collectibles;
        Medicine = medicine;
        ActionPoints = actionPoints;

        RaiseStatsChanged();
    }
}
