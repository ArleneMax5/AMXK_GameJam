using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIPanel : BasePanel
{
    [Header("״̬�� (Sliders)")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider sanitySlider;

    [Header("��ֵ�ı� (Texts)")]
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI foodText;
    [SerializeField] private TextMeshProUGUI collectiblesText;
    [SerializeField] private TextMeshProUGUI medicineText;

    [Header("�ж�������ʾ")]
    [SerializeField] private ActionPointDisplay actionPointDisplay;

    // �������屻����ʱ�����Ϳ�ʼ���������㲥
    private void OnEnable()
    {
        // ȷ��GameManager�Ѿ�����
        if (GameManager.Instance != null)
        {
            // �����¼����� OnStatsChanged �㲥����ʱ���������ǵ� UpdateDisplay ����
            GameManager.Instance.OnStatsChanged += UpdateDisplay;
            // ��������һ�Σ�����ʾ��ǰ״̬
            UpdateDisplay();
        }
    }

    // �������屻����ʱ������ֹͣ�����������Է�����
    private void OnDisable()
    {
        // ȷ��GameManagerʵ�������ڣ���ֹ����Ϸ�˳�ʱ����
        if (GameManager.Instance != null)
        {
            // ȡ������
            GameManager.Instance.OnStatsChanged -= UpdateDisplay;
        }
    }

    // ��������������¼��Զ����ã�������Ҫ���ⲿ�ֶ�����
    private void UpdateDisplay()
    {
        var gm = GameManager.Instance;

        if (healthSlider != null) healthSlider.value = gm.Health;
        if (hungerSlider != null) hungerSlider.value = gm.Hunger;
        if (sanitySlider != null) sanitySlider.value = gm.Sanity;

        if (dayText != null) dayText.text = $"����: {gm.CurrentDay}";
        if (foodText != null) foodText.text = $"{gm.Food}";
        if (collectiblesText != null) collectiblesText.text = $"{gm.Collectibles}";
        if (medicineText != null) medicineText.text = $"{gm.Medicine}";

        if (actionPointDisplay != null) actionPointDisplay.UpdatePoints(gm.ActionPoints);
    }
}