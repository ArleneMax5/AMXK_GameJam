using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �����ͼ UI ������
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
        // ע�� controller ����
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

        // ���߼���ֻ��ֹ�ظ������ǰ����
        if (target == currentRegion) return;
        // �Ƴ��˶���������ļ��

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
                if (!visible) continue; // ���ɼ��Ͳ��ٸ���ɫ
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
