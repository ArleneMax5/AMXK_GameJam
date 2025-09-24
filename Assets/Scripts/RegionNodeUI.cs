using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct RegionCost
{
    public int action, food, sanity, health;
}

public enum RegionType { 安全区_田地, 危险区_森林, 危险区_灾后废墟, 中危险区_灌木, 中危险区_养殖区, 营地休息 }

public class RegionNodeUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{


    [Header("Refs")]
    public Image fillImage;    // 填充图（必须）
    public Image borderImage;  // 边框图（必须，Raycast Target 关）

    [Header("标识与邻接")]
    public string regionId;
    public List<RegionNodeUI> neighbors = new List<RegionNodeUI>();

    [Header("前往该区块的消耗/信息")]
    public RegionType regionType;
    public RegionCost travelCost;
    [TextArea] public string regionDesc;      // 区域说明（表格右侧描述）
    [TextArea] public string rewardPreview;   // “可获得物资”的提示（例如：食物2 / 医疗1 …）

    // 可选：资源不够是否禁止进入
    public bool blockIfInsufficient = true;

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
