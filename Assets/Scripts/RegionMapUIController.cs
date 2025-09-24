using System.Collections.Generic;
using UnityEngine;

public class RegionMapUIController : MonoBehaviour
{
    [Header("区域集合与当前区域")]
    public List<RegionNodeUI> allRegions = new List<RegionNodeUI>();
    public RegionNodeUI currentRegion;

    [Header("填充配色（悬停不改变填充）")]
    public Color currentFillColor = new Color(0.35f, 0.75f, 1f, 1f); // 当前
    public Color dimFillColor = new Color(0.55f, 0.55f, 0.55f, 0.6f); // 其余

    [Header("边框配色")]
    public Color defaultBorderColor = new Color(1f, 1f, 1f, 0.35f);  // 常态（淡）
    public Color hoverBorderColor = new Color(1f, 0.85f, 0.1f, 1f); // 悬停（黄）
    public bool highlightCurrentBorder = true;
    public Color currentBorderColor = new Color(1f, 0.95f, 0.2f, 1f);

    [Header("仅显示当前+悬停（可选）")]
    public bool onlyShowCurrentAndHover = false;

    public RegionNodeUI HoverRegion { get; private set; }

    RegionNodeUI _pendingTarget;

    void Awake()
    {
        foreach (var r in allRegions) if (r) r.controller = this;
    }

    void OnEnable() => ApplyVisuals();

    public void SetHover(RegionNodeUI region)
    {
        if (HoverRegion == region) return;
        HoverRegion = region;
        ApplyVisuals();
    }

    public void TryEnter(RegionNodeUI target)
    {
        if (!target || target == currentRegion) return;
        if (currentRegion != null && !currentRegion.IsNeighborOf(target)) return;

        // 弹出确认面板，而不是立刻扣资源
        _pendingTarget = target;

        var panel = UIManager.Instance;  // UIManager 负责 PushPanel:contentReference[oaicite:4]{index=4}
        panel.PushPanel(PanelType.TravelConfirm);

        // 拿到刚压栈的面板实例并传参
        // 你 UIManager 里有 _panelInstances 字典（内部），这里可通过查找场景获取组件：
        var confirm = FindObjectOfType<TravelConfirmPanel>(true);
        if (confirm)
        {
            confirm.Setup(
                target,
                onConfirm: () => ConfirmTravel(),  // 点击“确认”
                onCancel: () => { _pendingTarget = null; } // 点击“取消”
            );
        }
    }

    void ConfirmTravel()
    {
        if (_pendingTarget == null) return;

        // 再次校验资源
        if (!_pendingTarget.CanAfford())
        {
            StartCoroutine(FlashBorder(_pendingTarget)); // 你原来的不足反馈
            _pendingTarget = null;
            return;
        }

        // 扣减资源 + 进入
        _pendingTarget.ApplyCost();           // 触发 GameManager 事件，GameUIPanel 会自动刷新:contentReference[oaicite:5]{index=5}
        currentRegion = _pendingTarget;
        _pendingTarget = null;
        ApplyVisuals();
    }

    void ApplyVisuals()
    {
        foreach (var node in allRegions)
        {
            if (!node) continue;

            // 显隐
            bool visible = !onlyShowCurrentAndHover || node == currentRegion || node == HoverRegion;
            node.SetVisible(visible);
            if (!visible) continue;

            // 填充：当前高亮，其余变暗（悬停不变填充）
            node.SetFillColor(node == currentRegion ? currentFillColor : dimFillColor);

            // 边框：默认=淡色；悬停=黄；当前可选高亮
            if (node == HoverRegion)
            {
                node.SetBorderColor(hoverBorderColor);
            }
            else if (highlightCurrentBorder && node == currentRegion)
            {
                node.SetBorderColor(currentBorderColor);
            }
            else
            {
                node.SetBorderColor(defaultBorderColor);
            }
        }
    }
}
