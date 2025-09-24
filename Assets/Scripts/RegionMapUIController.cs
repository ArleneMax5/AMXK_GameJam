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

    RegionNodeUI _pendingTarget;

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
        if (currentRegion != null && !currentRegion.IsNeighborOf(target)) return;

        // ����ȷ����壬���������̿���Դ
        _pendingTarget = target;

        var panel = UIManager.Instance;  // UIManager ���� PushPanel:contentReference[oaicite:4]{index=4}
        panel.PushPanel(PanelType.TravelConfirm);

        // �õ���ѹջ�����ʵ��������
        // �� UIManager ���� _panelInstances �ֵ䣨�ڲ����������ͨ�����ҳ�����ȡ�����
        var confirm = FindObjectOfType<TravelConfirmPanel>(true);
        if (confirm)
        {
            confirm.Setup(
                target,
                onConfirm: () => ConfirmTravel(),  // �����ȷ�ϡ�
                onCancel: () => { _pendingTarget = null; } // �����ȡ����
            );
        }
    }

    void ConfirmTravel()
    {
        if (_pendingTarget == null) return;

        // �ٴ�У����Դ
        if (!_pendingTarget.CanAfford())
        {
            StartCoroutine(FlashBorder(_pendingTarget)); // ��ԭ���Ĳ��㷴��
            _pendingTarget = null;
            return;
        }

        // �ۼ���Դ + ����
        _pendingTarget.ApplyCost();           // ���� GameManager �¼���GameUIPanel ���Զ�ˢ��:contentReference[oaicite:5]{index=5}
        currentRegion = _pendingTarget;
        _pendingTarget = null;
        ApplyVisuals();
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
