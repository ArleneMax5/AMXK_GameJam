using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionMapUIController : MonoBehaviour
{
    [Header("区域集合与当前区域")]
    public List<RegionNodeUI> allRegions = new List<RegionNodeUI>();
    public RegionNodeUI currentRegion;

    [Header("配色")]
    public Color currentColor = new Color(0.35f, 0.75f, 1f, 1f); // 当前
    public Color hoverColor = new Color(1f, 0.85f, 0.30f, 1f); // 悬停
    public Color dimColor = new Color(0.55f, 0.55f, 0.55f, 0.6f); // 其余变暗

    [Header("只显示当前与悬停，其余隐藏")]
    public bool onlyShowCurrentAndHover = false;

    public RegionNodeUI HoverRegion { get; private set; }

    void Awake()
    {
        // 注入 controller 引用
        foreach (var r in allRegions)
        {
            if (!r) continue;
            r.controller = this;
        }
    }

    void OnEnable()
    {
        ApplyColors();
    }

    public void SetHover(RegionNodeUI region)
    {
        if (HoverRegion == region) return;
        HoverRegion = region;
        ApplyColors();
    }

    public void TryEnter(RegionNodeUI target)
    {
        if (!target) return;

        // 只能点击进入相邻区域；点击自身则忽略
        if (target == currentRegion) return;
        if (currentRegion != null && !currentRegion.IsNeighborOf(target)) return;

        currentRegion = target;
        ApplyColors();
    }

    void ApplyColors()
    {
        foreach (var node in allRegions)
        {
            if (!node) continue;

            if (onlyShowCurrentAndHover)
            {
                bool visible = (node == currentRegion) || (node == HoverRegion);
                SetVisible(node, visible);
                if (!visible) continue; // 不可见就不再改颜色
            }

            if (node == currentRegion)
            {
                node.SetColor(currentColor);
            }
            else if (node == HoverRegion)
            {
                node.SetColor(hoverColor);
            }
            else
            {
                node.SetColor(dimColor);
            }
        }
    }

    void SetVisible(RegionNodeUI node, bool visible)
    {
        var g = node.GetComponent<Graphic>();
        if (g) g.enabled = visible;
    }
}
