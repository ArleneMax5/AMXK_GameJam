using System;
using UnityEngine;

// 管理游戏的核心数据和逻辑
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- 核心游戏数据（外部只读） ---
    public int CurrentDay { get; private set; }
    public int ActionPoints { get; private set; }
    public int Health { get; private set; }
    public int Hunger { get; private set; }
    public int Sanity { get; private set; }
    public int Food { get; private set; }
    public int Collectibles { get; private set; }
    public int Medicine { get; private set; }

    // --- 唯一的事件：UI 订阅它来刷新 ---
    public event Action OnStatsChanged;

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

    // 统一触发点
    private void RaiseStatsChanged() => OnStatsChanged?.Invoke();

    // 开始新游戏（例子）
    public void InitializeNewGame()
    {
        CurrentDay = 1;
        ActionPoints = 8;
        Health = 100;
        Hunger = 100;
        Sanity = 100;
        Food = 0;
        Collectibles = 0;
        Medicine = 0;

        RaiseStatsChanged();
    }

    // 示例逻辑
    public void EndDay()
    {
        CurrentDay++;
        ActionPoints = 8;
        Hunger -= 1;
        RaiseStatsChanged();
    }

    public void UseActionPoints(int amount)
    {
        if (ActionPoints >= amount)
        {
            ActionPoints -= amount;
            RaiseStatsChanged();
        }
    }

    public void AddFood(int amount)
    {
        Food += amount;
        RaiseStatsChanged();
    }

    // ==== 读档恢复入口（供 SaveLoadManager 调用） ====
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
