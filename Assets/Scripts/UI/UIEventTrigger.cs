using UnityEngine;

// ���С�ű������ڰ�ť�ϣ�������Inspector��������Ҫ�������¼�����
public class UIEventTrigger : MonoBehaviour
{
    [SerializeField] private GameEventType eventType;

    // �����������������ť�� OnActivated �¼�����
    public void Trigger()
    {
        GameEventManager.Instance.TriggerEvent(eventType);
    }
}