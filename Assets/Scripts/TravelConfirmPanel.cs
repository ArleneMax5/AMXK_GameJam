using System;
using UnityEngine;
using TMPro;

public class TravelConfirmPanel : BasePanel
{
    [Header("UI Refs")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI descText;

    // 由控制器注入
    Action _onConfirm;
    Action _onCancel;

    public void Setup(RegionNodeUI target, Action onConfirm, Action onCancel)
    {
        _onConfirm = onConfirm;
        _onCancel = onCancel;

        if (titleText) titleText.text = $"前往：{target.regionType}";
        if (costText) costText.text =
            $"消耗行动点：{target.travelCost.action}\n" +
            $"消耗饥饿值：{target.travelCost.food}\n" +
            $"消耗精神值：{target.travelCost.sanity}\n" +
            $"消耗生命值：{target.travelCost.health}";
        if (rewardText) rewardText.text = string.IsNullOrEmpty(target.rewardPreview) ? "—" : target.rewardPreview;
        if (descText) descText.text = string.IsNullOrEmpty(target.regionDesc) ? "—" : target.regionDesc;
    }

    // 绑定到两个按钮的 OnClick
    public void OnClickConfirm() { _onConfirm?.Invoke(); UIManager.Instance.PopPanel(); }
    public void OnClickCancel() { _onCancel?.Invoke(); UIManager.Instance.PopPanel(); }
}
