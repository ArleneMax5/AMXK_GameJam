using System;
using UnityEngine;

// ������Ϸ�ĺ������ݺ��߼�
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- ������Ϸ���� ---
    // ʹ�����ԣ�Property������װ���ݣ��ⲿֻ�ܶ�ȡ��ֻ����GameManager�ڲ��޸�
    public int CurrentDay { get; private set; }
    public int ActionPoints { get; private set; }
    public int Health { get; private set; }
    public int Hunger { get; private set; }
    public int Sanity { get; private set; }
    public int Food { get; private set; }
    public int Collectibles { get; private set; }
    public int Medicine { get; private set; }

    // --- �¼������κ����ݷ����仯ʱ���ͷ�������㲥 ---
    public event Action OnStatsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ȷ��GameManager���л�����ʱ���ᱻ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ����ʼһ������Ϸʱ���� GameEventManager ����
    public void InitializeNewGame()
    {
        // ��ʼ����������
        CurrentDay = 1;
        ActionPoints = 8; // ����ÿ����8���ж���
        Health = 100;   
        Hunger = 100;
        Sanity = 100;
        Food = 0;
        Collectibles = 0;
        Medicine = 0;

        // ��ʼ����ɺ������㲥һ�Σ���UI��ʾ��ʼ״̬
        OnStatsChanged?.Invoke();
    }

    // --- �����߼�����ʾ�� ---

    public void EndDay()
    {
        CurrentDay++;
        ActionPoints = 8; // �����ж���
        Hunger -= 1;   // ÿ�����ļ���ֵ

        // ���ݱ仯�󣬹㲥֪ͨ
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