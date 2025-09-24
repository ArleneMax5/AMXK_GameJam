using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 区域地图 UI 控制器
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

        // 简化逻辑：只防止重复点击当前区域
        if (target == currentRegion) return;
        // 移除了对相邻区域的检查

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