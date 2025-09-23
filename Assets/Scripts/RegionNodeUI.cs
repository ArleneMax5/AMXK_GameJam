using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RegionNodeUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("��ʶ")]
    public string regionId;

    [Header("�ڽ�����ֻ�ܵ����Щ��")]
    public List<RegionNodeUI> neighbors = new List<RegionNodeUI>();

    // �ɿ�����ע��
    [HideInInspector] public RegionMapUIController controller;

    Image _img;

    void Awake()
    {
        _img = GetComponent<Image>();
    }

    public bool IsNeighborOf(RegionNodeUI other) => neighbors.Contains(other);

    // ͳһ���ñ����򣨼�����ͼƬ������ɫ
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
        // ������뿪��û�н���������ʱ�����ͣ
        if (controller && controller.HoverRegion == this)
            controller.SetHover(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller?.TryEnter(this);
    }
}
