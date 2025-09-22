using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 这是一个可复用的基础脚本，任何需要按钮导航的面板都可以继承它
public class BaseNavigablePanel : MonoBehaviour
{
    [Header("面板内的按钮列表")]
    [SerializeField] protected List<SelectableButton> buttons;

    protected int currentIndex = -1;
    private EventSystem eventSystem;
    private bool isNavigationActive = false;

    protected virtual void Awake()
    {
        eventSystem = EventSystem.current;
    }

    // 当面板被激活时，由 UIManager 调用
    public virtual void OnPanelActive()
    {
        isNavigationActive = true;
        if (buttons != null && buttons.Count > 0)
        {
            SelectButton(0); // 默认选中第一个
        }
    }

    // 当面板被隐藏时，由 UIManager 调用
    public virtual void OnPanelInactive()
    {
        isNavigationActive = false;
        if (currentIndex != -1 && currentIndex < buttons.Count)
        {
            buttons[currentIndex].OnDeselected(); // 取消选中效果
        }
        currentIndex = -1;
    }

    protected virtual void Update()
    {
        if (!isNavigationActive) return; // 如果导航未激活，则不执行任何操作

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

        // --- 激活按钮 (回车/空格/鼠标点击) ---
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
        int newIndex = (currentIndex == -1) ? 0 : currentIndex + direction;
        if (newIndex < 0) { newIndex = buttons.Count - 1; }
        else if (newIndex >= buttons.Count) { newIndex = 0; }
        SelectButton(newIndex);
    }

    void SelectButton(int index)
    {
        if (index < 0 || index >= buttons.Count) return;

        if (currentIndex != -1 && currentIndex != index && currentIndex < buttons.Count)
        {
            buttons[currentIndex].OnDeselected();
        }

        currentIndex = index;
        buttons[currentIndex].OnSelected();
        eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
    }
}