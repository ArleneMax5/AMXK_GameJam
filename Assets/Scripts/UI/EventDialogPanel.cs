using UnityEngine;
using TMPro;

public class EventDialogPanel : BasePanel
{
    [Header("事件对话框 UI 元素")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    // 由 UIManager 调用，用于显示一个特定的事件
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

        // 如果有选项按钮，可以在这里根据 gameEvent.choices 来配置它们
        // 例如：buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = gameEvent.choices[0].text;
    }
}