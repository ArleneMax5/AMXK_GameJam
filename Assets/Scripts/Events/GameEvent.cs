using UnityEngine;

// 创建一个 GameEvent 数据结构，并让 EventDialogPanel 的 ShowEvent 方法接收这个数据对象来填充UI。
[CreateAssetMenu(fileName = "New GameEvent", menuName = "Game/Game Event")]
public class GameEvent : ScriptableObject
{
    [Header("事件信息")]
    public string title;
    [TextArea(3, 10)]
    public string description;

    // 这里可以扩展更多事件相关的属性，例如：
    // public Sprite eventImage;
    // public Choice[] choices;
}