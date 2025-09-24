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
        if (currentRegion != null && !currentRegion.IsNeighborOf(target)) return; // 只允许相邻
        currentRegion = target;
        ApplyVisuals();
        // 如需弹事件面板：UIManager.Instance.PushPanel(PanelType.EventDialog);
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
