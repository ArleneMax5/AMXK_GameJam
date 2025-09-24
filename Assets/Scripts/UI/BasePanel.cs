using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
// 总结：这个 BasePanel 类实现了统一的键盘导航、鼠标悬停选择和点击确认按钮的逻辑。

// public enum PanelType
// {
//     MainMenu,
//     GameUI,
//     PauseMenu,
//     EventDialog,
//     Tutorial,
//     // 新增指定
//     Interaction,
//     // 事件交互
//     EventResult,
//     // 新增失败面板
//     GameOver
// }


[RequireComponent(typeof(Canvas))] // 强制要求每个面板都有一个Canvas组件
public class BasePanel : MonoBehaviour
{
    // 让每个面板知道自己的类型
    [SerializeField] private PanelType panelType;
    public PanelType PanelType => panelType;

    [Header("面板内的按钮列表")]
    [SerializeField] protected List<SelectableButton> buttons;

    protected int currentIndex = -1;
    private bool isNavigationActive = false;
    private EventSystem eventSystem;
    private Canvas panelCanvas; // 新增：对Canvas组件的引用

    protected virtual void Awake()
    {
        eventSystem = EventSystem.current;
        panelCanvas = GetComponent<Canvas>(); // 获取Canvas组件
        if (panelCanvas == null)
        {
            Debug.LogError($"面板 {gameObject.name} 缺少 Canvas 组件！", this);
        }
    }

    // 作为面板被激活时，由UIManager调用
    public virtual void Show()
    {
        gameObject.SetActive(true);
        isNavigationActive = true;
        if (buttons != null && buttons.Count > 0)
        {
            SelectButton(0); // 默认选中第一个
        }
    }

    // 作为面板被隐藏时，由UIManager调用
    public virtual void Hide()
    {
        isNavigationActive = false;
        if (currentIndex != -1 && currentIndex < buttons.Count)
        {
            buttons[currentIndex].OnDeselected(); // 取消选中效果
        }
        currentIndex = -1;
        gameObject.SetActive(false);
    }

    // 修改：移除了对 overrideSorting 的设置，因为它只用于嵌套Canvas
    public void SetSortOrder(int order)
    {
        if (panelCanvas != null)
        {
            // 对于根Canvas，我们只需要直接设置 sortingOrder
            panelCanvas.sortingOrder = order;
        }
    }

    // 只处理键盘导航
    protected virtual void Update()
    {
        if (!isNavigationActive || buttons == null || buttons.Count == 0) return;

        // --- 键盘导航 ---
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(1);
        }

        // --- 键盘确认按钮 (回车/空格) ---
        if (currentIndex != -1 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space)))
        {
            buttons[currentIndex].ActivateButton();
        }
    }

    void ChangeSelection(int direction)
    {
        if (buttons == null || buttons.Count == 0) return;

        // 如果之前没有选中任何按钮，则从第一个开始
        int newIndex = (currentIndex == -1) ? 0 : currentIndex + direction;

        // 实现循环选择
        if (newIndex < 0) { newIndex = buttons.Count - 1; }
        else if (newIndex >= buttons.Count) { newIndex = 0; }

        SelectButton(newIndex);
    }

    // 公开此方法，以便 SelectableButton 可以调用它
    public void SelectButton(SelectableButton button)
    {
        int index = buttons.IndexOf(button);
        if (index != -1)
        {
            SelectButton(index);
        }
    }

    void SelectButton(int index)
    {
        if (index < 0 || index >= buttons.Count) return;

        // 如果选中了新的按钮，则取消上一个按钮的选中状态
        if (currentIndex != -1 && currentIndex != index && currentIndex < buttons.Count)
        {
            buttons[currentIndex].OnDeselected();
        }

        currentIndex = index;
        buttons[currentIndex].OnSelected();

        // 更新 EventSystem 的选中对象，以便直接支持需要它的某些UI功能
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
    }
}