using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ����һ���ɸ��õĻ����ű����κ���Ҫ��ť��������嶼���Լ̳���
public class BaseNavigablePanel : MonoBehaviour
{
    [Header("����ڵİ�ť�б�")]
    [SerializeField] protected List<SelectableButton> buttons;

    protected int currentIndex = -1;
    private EventSystem eventSystem;
    private bool isNavigationActive = false;

    protected virtual void Awake()
    {
        eventSystem = EventSystem.current;
    }

    // ����屻����ʱ���� UIManager ����
    public virtual void OnPanelActive()
    {
        isNavigationActive = true;
        if (buttons != null && buttons.Count > 0)
        {
            SelectButton(0); // Ĭ��ѡ�е�һ��
        }
    }

    // ����屻����ʱ���� UIManager ����
    public virtual void OnPanelInactive()
    {
        isNavigationActive = false;
        if (currentIndex != -1 && currentIndex < buttons.Count)
        {
            buttons[currentIndex].OnDeselected(); // ȡ��ѡ��Ч��
        }
        currentIndex = -1;
    }

    protected virtual void Update()
    {
        if (!isNavigationActive) return; // �������δ�����ִ���κβ���

        // --- ���̵��� ---
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(1);
        }

        // --- �����ͣѡ�� ---
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

        // --- ���ť (�س�/�ո�/�����) ---
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