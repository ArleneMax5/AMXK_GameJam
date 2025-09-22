using UnityEngine;
using TMPro;

public class EventDialogPanel : BaseNavigablePanel
{
    [Header("�¼��Ի��� UI ����")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    // ����������� UIManager ���ã�������ʾһ���ض����¼�
    public void ShowEvent(/* ������Դ���һ���¼����ݶ��� */)
    {
        // ʾ����
        titleText.text = "һ������¼�";
        descriptionText.text = "�㷢����һ�����䣬�����ƺ��ж�����";

        // �����һ����ť�ǡ��򿪡����ڶ����ǡ��뿪��
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "�򿪱���";
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "������";

        // �����
        OnPanelActive();
    }
}