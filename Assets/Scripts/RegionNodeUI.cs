using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RegionNodeUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("标识")]
    public string regionId;

    [Header("邻接区域（只能点击这些）")]
    public List<RegionNodeUI> neighbors = new List<RegionNodeUI>();

    // 由控制器注入
    [HideInInspector] public RegionMapUIController controller;

    Image _img;

    void Awake()
    {
        _img = GetComponent<Image>();
    }

    public bool IsNeighborOf(RegionNodeUI other) => neighbors.Contains(other);

    // 统一设置本区域（及额外图片）的颜色
    public void SetColor(Color c)
    {
        if (_img) _img.color = c;
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        controller?.SetHover(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 当鼠标离开且没有进入别的区域时清空悬停
        if (controller && controller.HoverRegion == this)
            controller.SetHover(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller?.TryEnter(this);
    }
}
