using System;
using UnityEngine;

// 管理游戏的核心数据和逻辑
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("快进设置")]
    [SerializeField] private int dailyActionPoints = 10;
    [SerializeField] private int dailyHungerLoss = 5;
    [SerializeField] private int dailySanityLoss = 5;
    [SerializeField] private int minHunger = 0;
    [SerializeField] private int minSanity = 0;

    // --- 核心游戏数据（外部只读） ---
    public int CurrentDay { get; private set; }
    public int ActionPoints { get; private set; }
    public int Health { get; private set; }
    public int Hunger { get; private set; }
    public int Sanity { get; private set; }
    public int Food { get; private set; }
    public int Collectibles { get; private set; }
    public int Medicine { get; private set; }

    // 最大行动点（供 UI 读取）
    public int MaxActionPoints => dailyActionPoints;

    // --- 游戏状态 ---
    public bool IsGameOver { get; private set; }

    // --- 事件 ---
    public event Action OnStatsChanged;
    public event Action<int> OnDayChanged; // 新增：天数变化事件

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

    // 由 GameOverPanel 调用
    public void RestartGame()
    {
        IsGameOver = false;
        InitializeNewGame();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ClearAndPushPanel(PanelType.MainMenu);
        }
    }

    // 初始化新游戏
    public void InitializeNewGame()
    {
        CurrentDay = 1;
        ActionPoints = dailyActionPoints; // 假设您有 dailyActionPoints 变量
        Health = 100;
        Hunger = 100;
        Sanity = 100;
        Food = 0;
        Collectibles = 0;
        Medicine = 0;
        IsGameOver = false;

        RaiseStatsChanged();
        // 当开始新游戏时，也应该触发天数变化事件，以确保音乐正确播放
        OnDayChanged?.Invoke(CurrentDay);
    }

    public void FastForwardDay()
    {
        if (IsGameOver) return;

        CurrentDay++;
        ActionPoints = dailyActionPoints;
        
        // --- 修改点 ---
        // 使用序列化的变量来减少饥饿和精神值
        Hunger = Mathf.Max(minHunger, Hunger - dailyHungerLoss);
        Sanity = Mathf.Max(minSanity, Sanity - dailySanityLoss);
        // ------------

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("FastForward");
        }

        RaiseStatsChanged();
        
        // 在天数改变后，立即触发 OnDayChanged 事件，并把新的天数传递出去
        OnDayChanged?.Invoke(CurrentDay);

        CheckGameOver();
    }

    // 消耗行动点（供按钮或逻辑调用）
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
        // 从存档加载时，也应该触发天数变化事件
        OnDayChanged?.Invoke(CurrentDay);
        CheckGameOver();
    }   

    /// <summary>
    /// 使用药品，恢复20点生命值
    /// </summary>
    public void UseMedicine()
    {
        if (Medicine > 0)
        {
            Medicine--;
            Health = Mathf.Min(Health + 20, 100); // 假设最大值为100
            RaiseStatsChanged();
        }
    }

    /// <summary>
    /// 使用食物，恢复10点饥饿值
    /// </summary>
    public void UseFood()
    {
        if (Food > 0)
        {
            Food--;
            Hunger = Mathf.Min(Hunger + 10, 100); // 假设最大值为100
            RaiseStatsChanged();
        }
    }

    /// <summary>
    /// 使用收藏品，恢复10点精神值
    /// </summary>
    public void UseCollectible()
    {
        if (Collectibles > 0)
        {
            Collectibles--;
            Sanity = Mathf.Min(Sanity + 10, 100); // 假设最大值为100
            RaiseStatsChanged();
        }
    }
}