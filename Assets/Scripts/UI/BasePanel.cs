using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// 总结：这个 BasePanel 类现在整合了键盘导航、鼠标悬停选择和点击激活按钮的完整逻辑。
public class BasePanel : MonoBehaviour
{
    // 新增：让每个面板都知道自己的类型
    [SerializeField] private PanelType panelType;
    public PanelType PanelType => panelType;

    [Header("面板内的按钮列表")]
    [SerializeField] protected List<SelectableButton> buttons;

    protected int currentIndex = -1;
    private bool isNavigationActive = false;
    private EventSystem eventSystem; // --- 新增: 引用 EventSystem ---

    // --- 新增: Awake 方法用于获取 EventSystem ---
    protected virtual void Awake()
    {
        eventSystem = EventSystem.current;
    }

    // 改为公共方法，由UIManager调用
    public virtual void Show()
    {
        gameObject.SetActive(true);
        isNavigationActive = true;
        if (buttons != null && buttons.Count > 0)
        {
            SelectButton(0); // 默认选中第一个
        }
    }

    // 改为公共方法，由UIManager调用
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

    // --- 核心修改: 整合了键盘、鼠标悬停和点击的完整导航逻辑 ---
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

        // --- 鼠标悬停选择 ---
        PointerEventData pointerData = new PointerEventData(eventSystem) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerData, results);

        SelectableButton hoveredButton = null;
        if (results.Count > 0)
        {
            hoveredButton = results[0].gameObject.GetComponentInParent<SelectableButton>();
        }

        if (hoveredButton != null && buttons.Contains(hoveredButton))
        {
            int newIndex = buttons.IndexOf(hoveredButton);
            if (newIndex != currentIndex)
            {
                SelectButton(newIndex);
            }
        }

        // --- 激活按钮 (回车/空格/鼠标左键) ---
        if (currentIndex != -1)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                buttons[currentIndex].ActivateButton();
            }
            else if (Input.GetMouseButtonDown(0) && hoveredButton == buttons[currentIndex])
            {
                buttons[currentIndex].ActivateButton();
            }
        }
    }

    void ChangeSelection(int direction)
    {
        if (buttons == null || buttons.Count == 0) return;

        // 如果之前没有选中任何按钮，则从第一个开始
        int newIndex = (currentIndex == -1) ? 0 : currentIndex + direction;

        // 处理循环选择
        if (newIndex < 0) { newIndex = buttons.Count - 1; }
        else if (newIndex >= buttons.Count) { newIndex = 0; }

        SelectButton(newIndex);
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

        // 更新 EventSystem 的选中对象，这对于手柄支持和某些UI交互很重要
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
    }
}