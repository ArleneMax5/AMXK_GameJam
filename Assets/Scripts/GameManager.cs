using System;
using UnityEngine;

// 管理游戏的核心数据和逻辑
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- 核心游戏数据 ---
    // 使用属性（Property）来封装数据，外部只能读取，只能由GameManager内部修改
    public int CurrentDay { get; private set; }
    public int ActionPoints { get; private set; }
    public int Health { get; private set; }
    public int Hunger { get; private set; }
    public int Sanity { get; private set; }
    public int Food { get; private set; }
    public int Collectibles { get; private set; }
    public int Medicine { get; private set; }

    // --- 事件：当任何数据发生变化时，就发出这个广播 ---
    public event Action OnStatsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保GameManager在切换场景时不会被销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 当开始一个新游戏时，由 GameEventManager 调用
    public void InitializeNewGame()
    {
        // 初始化所有数据
        CurrentDay = 1;
        ActionPoints = 8; // 假设每天有8个行动点
        Health = 100;   
        Hunger = 100;
        Sanity = 100;
        Food = 0;
        Collectibles = 0;
        Medicine = 0;

        // 初始化完成后，立即广播一次，让UI显示初始状态
        OnStatsChanged?.Invoke();
    }

    // --- 核心逻辑方法示例 ---

    public void EndDay()
    {
        CurrentDay++;
        ActionPoints = 8; // 重置行动点
        Hunger -= 1;   // 每天消耗饥饿值

        // 数据变化后，广播通知
        OnStatsChanged?.Invoke();
    }

    public void UseActionPoints(int amount)
    {
        if (ActionPoints >= amount)
        {
            ActionPoints -= amount;
            OnStatsChanged?.Invoke();
        }
    }

    public void AddFood(int amount)
    {
        Food += amount;
        OnStatsChanged?.Invoke();
    }
}