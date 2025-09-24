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

    // �ɿ�����ע��
    Action _onConfirm;
    Action _onCancel;

    public void Setup(RegionNodeUI target, Action onConfirm, Action onCancel)
    {
        _onConfirm = onConfirm;
        _onCancel = onCancel;

        if (titleText) titleText.text = $"ǰ����{target.regionType}";
        if (costText) costText.text =
            $"�����ж��㣺{target.travelCost.action}\n" +
            $"���ļ���ֵ��{target.travelCost.food}\n" +
            $"���ľ���ֵ��{target.travelCost.sanity}\n" +
            $"��������ֵ��{target.travelCost.health}";
        if (rewardText) rewardText.text = string.IsNullOrEmpty(target.rewardPreview) ? "��" : target.rewardPreview;
        if (descText) descText.text = string.IsNullOrEmpty(target.regionDesc) ? "��" : target.regionDesc;
    }

    // �󶨵�������ť�� OnClick
    public void OnClickConfirm() { _onConfirm?.Invoke(); UIManager.Instance.PopPanel(); }
    public void OnClickCancel() { _onCancel?.Invoke(); UIManager.Instance.PopPanel(); }
}
