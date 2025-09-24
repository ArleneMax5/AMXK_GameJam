using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegionNodeUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Refs")]
    public Image fillImage;    // ���ͼ�����룩
    public Image borderImage;  // �߿�ͼ�����룬Raycast Target �أ�

    [Header("��ʶ���ڽ�")]
    public string regionId;
    public List<RegionNodeUI> neighbors = new List<RegionNodeUI>();

    [HideInInspector] public RegionMapUIController controller;

    void Reset()
    {
        // �����Ͻű�ʱ�Զ����ӽڵ�
        if (!fillImage) fillImage = transform.Find("Fill")?.GetComponent<Image>();
        if (!borderImage) borderImage = transform.Find("Border")?.GetComponent<Image>();
    }

    public bool IsNeighborOf(RegionNodeUI other) => neighbors.Contains(other);

    // ���� �����������õ��Ӿ��ӿ� ����
    public void SetFillColor(Color c) { if (fillImage) fillImage.color = c; }
    public void SetBorderColor(Color c) { if (borderImage) borderImage.color = c; }
    public void SetVisible(bool on)
    {
        if (fillImage) fillImage.enabled = on;
        if (borderImage) borderImage.enabled = on;
    }

    // ���� ��ͣ/����¼� ����
    public void OnPointerEnter(PointerEventData e) { controller?.SetHover(this); }
    public void OnPointerExit(PointerEventData e) { if (controller?.HoverRegion == this) controller.SetHover(null); }
    public void OnPointerClick(PointerEventData e) { controller?.TryEnter(this); }
}
