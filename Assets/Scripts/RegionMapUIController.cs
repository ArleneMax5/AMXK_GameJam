using System.Collections.Generic;
using UnityEngine;

public class RegionMapUIController : MonoBehaviour
{
    [Header("���򼯺��뵱ǰ����")]
    public List<RegionNodeUI> allRegions = new List<RegionNodeUI>();
    public RegionNodeUI currentRegion;

    [Header("�����ɫ����ͣ���ı���䣩")]
    public Color currentFillColor = new Color(0.35f, 0.75f, 1f, 1f); // ��ǰ
    public Color dimFillColor = new Color(0.55f, 0.55f, 0.55f, 0.6f); // ����

    [Header("�߿���ɫ")]
    public Color defaultBorderColor = new Color(1f, 1f, 1f, 0.35f);  // ��̬������
    public Color hoverBorderColor = new Color(1f, 0.85f, 0.1f, 1f); // ��ͣ���ƣ�
    public bool highlightCurrentBorder = true;
    public Color currentBorderColor = new Color(1f, 0.95f, 0.2f, 1f);

    [Header("����ʾ��ǰ+��ͣ����ѡ��")]
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
        if (currentRegion != null && !currentRegion.IsNeighborOf(target)) return; // ֻ��������
        currentRegion = target;
        ApplyVisuals();
        // ���赯�¼���壺UIManager.Instance.PushPanel(PanelType.EventDialog);
    }

    void ApplyVisuals()
    {
        foreach (var node in allRegions)
        {
            if (!node) continue;

            // ����
            bool visible = !onlyShowCurrentAndHover || node == currentRegion || node == HoverRegion;
            node.SetVisible(visible);
            if (!visible) continue;

            // ��䣺��ǰ����������䰵����ͣ������䣩
            node.SetFillColor(node == currentRegion ? currentFillColor : dimFillColor);

            // �߿�Ĭ��=��ɫ����ͣ=�ƣ���ǰ��ѡ����
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
