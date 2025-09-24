using System;
using UnityEngine;

// ������Ϸ�ĺ������ݺ��߼�
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- ������Ϸ���ݣ��ⲿֻ���� ---
    public int CurrentDay { get; private set; }
    public int ActionPoints { get; private set; }
    public int Health { get; private set; }
    public int Hunger { get; private set; }
    public int Sanity { get; private set; }
    public int Food { get; private set; }
    public int Collectibles { get; private set; }
    public int Medicine { get; private set; }

    // --- Ψһ���¼���UI ��������ˢ�� ---
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

    // ͳһ������
    private void RaiseStatsChanged() => OnStatsChanged?.Invoke();

    // ��ʼ����Ϸ�����ӣ�
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

    // ʾ���߼�
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
