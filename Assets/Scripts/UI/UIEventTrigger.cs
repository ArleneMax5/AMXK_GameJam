using UnityEngine;

// 这个小脚本挂载在按钮上，用于在Inspector中设置它要触发的事件类型
public class UIEventTrigger : MonoBehaviour
{
    [SerializeField] private GameEventType eventType;

    // 这个公共方法将被按钮的 OnActivated 事件调用
    public void Trigger()
    {
        GameEventManager.Instance.TriggerEvent(eventType);
    }
}