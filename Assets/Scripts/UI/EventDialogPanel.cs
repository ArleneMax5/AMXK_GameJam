using UnityEngine;
using TMPro;

public class EventDialogPanel : BaseNavigablePanel
{
    [Header("事件对话框 UI 引用")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    // 这个方法将由 UIManager 调用，用来显示一个特定的事件
    public void ShowEvent(/* 这里可以传入一个事件数据对象 */)
    {
        // 示例：
        titleText.text = "一个随机事件";
        descriptionText.text = "你发现了一个宝箱，里面似乎有东西。";

        // 假设第一个按钮是“打开”，第二个是“离开”
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "打开宝箱";
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "无视它";

        // 激活导航
        OnPanelActive();
    }
}