using UnityEngine;
using TMPro;

public class EventDialogPanel : BasePanel
{
    [Header("�¼��Ի��� UI Ԫ��")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    // �� UIManager ���ã�������ʾһ���ض����¼�
    public void ShowEvent(GameEvent gameEvent)
    {
        if (gameEvent == null) return;

        if (titleText != null)
        {
            titleText.text = gameEvent.title;
        }
        if (descriptionText != null)
        {
            descriptionText.text = gameEvent.description;
        }

        // �����ѡ�ť��������������� gameEvent.choices ����������
        // ���磺buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = gameEvent.choices[0].text;
    }
}