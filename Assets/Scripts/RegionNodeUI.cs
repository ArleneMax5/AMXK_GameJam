using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class RegionNodeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("标识")]
    public string regionId;

    // [邻接区域] 相关的字段和方法已被移除

    [Header("音效设置")]
    [SerializeField] private string hoverSoundName = "RegionHover";
    [SerializeField] private string clickSoundName = "RegionClick";
    // [无效点击音效] 字段已被移除

    // 可控制器注入
    [HideInInspector] public RegionMapUIController controller;

    Image _img;

    void Awake()
    {
        _img = GetComponent<Image>();
    }

    // IsNeighborOf 方法已被移除

    // 统一设置背景色（即设置图片组件颜色）
    public void SetColor(Color c)
    {
        if (_img) _img.color = c;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        controller?.SetHover(this);

        // 播放悬停音效
        if (AudioManager.Instance != null && !string.IsNullOrEmpty(hoverSoundName))
        {
            AudioManager.Instance.PlaySFX(hoverSoundName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        controller?.SetHover(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller == null) return;

        // 只要点击的不是当前区域，就视为有效点击
        if (this != controller.currentRegion)
        {
            // 播放成功点击音效
            if (AudioManager.Instance != null && !string.IsNullOrEmpty(clickSoundName))
            {
                AudioManager.Instance.PlaySFX(clickSoundName);
            }
            // 尝试进入新区域
            controller.TryEnter(this);
        }
        // 无效点击的逻辑和音效已被完全移除
    }
}