using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIPanel : BasePanel
{
    [Header("状态条 (Sliders)")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider sanitySlider;

    [Header("数值文本 (Texts)")]
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI foodText;
    [SerializeField] private TextMeshProUGUI collectiblesText;
    [SerializeField] private TextMeshProUGUI medicineText;

    [Header("行动点数显示")]
    [SerializeField] private ActionPointDisplay actionPointDisplay;

    // 当这个面板被激活时，它就开始“收听”广播
    private void OnEnable()
    {
        // 确保GameManager已经存在
        if (GameManager.Instance != null)
        {
            // 订阅事件：当 OnStatsChanged 广播发出时，调用我们的 UpdateDisplay 方法
            GameManager.Instance.OnStatsChanged += UpdateDisplay;
            // 立即更新一次，以显示当前状态
            UpdateDisplay();
        }
    }

    // 当这个面板被隐藏时，它就停止“收听”，以防出错
    private void OnDisable()
    {
        // 确保GameManager实例还存在（防止在游戏退出时出错）
        if (GameManager.Instance != null)
        {
            // 取消订阅
            GameManager.Instance.OnStatsChanged -= UpdateDisplay;
        }
    }

    // 这个方法现在由事件自动调用，不再需要从外部手动调用
    private void UpdateDisplay()
    {
        var gm = GameManager.Instance;

        if (healthSlider != null) healthSlider.value = gm.Health;
        if (hungerSlider != null) hungerSlider.value = gm.Hunger;
        if (sanitySlider != null) sanitySlider.value = gm.Sanity;

        if (dayText != null) dayText.text = $"天数: {gm.CurrentDay}";
        if (foodText != null) foodText.text = $"{gm.Food}";
        if (collectiblesText != null) collectiblesText.text = $"{gm.Collectibles}";
        if (medicineText != null) medicineText.text = $"{gm.Medicine}";

        if (actionPointDisplay != null) actionPointDisplay.UpdatePoints(gm.ActionPoints);
    }
}