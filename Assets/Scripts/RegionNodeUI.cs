using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegionNodeUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Refs")]
    public Image fillImage;    // 填充图（必须）
    public Image borderImage;  // 边框图（必须，Raycast Target 关）

    [Header("标识与邻接")]
    public string regionId;
    public List<RegionNodeUI> neighbors = new List<RegionNodeUI>();

    [HideInInspector] public RegionMapUIController controller;

    void Reset()
    {
        // 方便拖脚本时自动找子节点
        if (!fillImage) fillImage = transform.Find("Fill")?.GetComponent<Image>();
        if (!borderImage) borderImage = transform.Find("Border")?.GetComponent<Image>();
    }

    public bool IsNeighborOf(RegionNodeUI other) => neighbors.Contains(other);

    // —— 供控制器调用的视觉接口 ——
    public void SetFillColor(Color c) { if (fillImage) fillImage.color = c; }
    public void SetBorderColor(Color c) { if (borderImage) borderImage.color = c; }
    public void SetVisible(bool on)
    {
        if (fillImage) fillImage.enabled = on;
        if (borderImage) borderImage.enabled = on;
    }

    // —— 悬停/点击事件 ——
    public void OnPointerEnter(PointerEventData e) { controller?.SetHover(this); }
    public void OnPointerExit(PointerEventData e) { if (controller?.HoverRegion == this) controller.SetHover(null); }
    public void OnPointerClick(PointerEventData e) { controller?.TryEnter(this); }
}
