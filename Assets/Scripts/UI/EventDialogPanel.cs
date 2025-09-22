using UnityEngine;
using TMPro;

public class EventDialogPanel : BasePanel
{
    [Header("事件对话框 UI 引用")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    // 这个方法将由 UIManager 调用，用来显示一个特定的事件
    public void ShowEvent(/* 这里可以传入一个事件数据对象 */)
    {
    }
}