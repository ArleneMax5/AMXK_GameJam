using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// �ܽ᣺��� BasePanel �����������˼��̵����������ͣѡ��͵�����ť�������߼���
public class BasePanel : MonoBehaviour
{
    // ��������ÿ����嶼֪���Լ�������
    [SerializeField] private PanelType panelType;
    public PanelType PanelType => panelType;

    [Header("����ڵİ�ť�б�")]
    [SerializeField] protected List<SelectableButton> buttons;

    protected int currentIndex = -1;
    private bool isNavigationActive = false;
    private EventSystem eventSystem; // --- ����: ���� EventSystem ---

    // --- ����: Awake �������ڻ�ȡ EventSystem ---
    protected virtual void Awake()
    {
        eventSystem = EventSystem.current;
    }

    // ��Ϊ������������UIManager����
    public virtual void Show()
    {
        gameObject.SetActive(true);
        isNavigationActive = true;
        if (buttons != null && buttons.Count > 0)
        {
            SelectButton(0); // Ĭ��ѡ�е�һ��
        }
    }

    // ��Ϊ������������UIManager����
    public virtual void Hide()
    {
        isNavigationActive = false;
        if (currentIndex != -1 && currentIndex < buttons.Count)
        {
            buttons[currentIndex].OnDeselected(); // ȡ��ѡ��Ч��
        }
        currentIndex = -1;
        gameObject.SetActive(false);
    }

    // --- �����޸�: �����˼��̡������ͣ�͵�������������߼� ---
    protected virtual void Update()
    {
        if (!isNavigationActive || buttons == null || buttons.Count == 0) return;

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

        // --- ���ť (�س�/�ո�/������) ---
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

        // ���֮ǰû��ѡ���κΰ�ť����ӵ�һ����ʼ
        int newIndex = (currentIndex == -1) ? 0 : currentIndex + direction;

        // ����ѭ��ѡ��
        if (newIndex < 0) { newIndex = buttons.Count - 1; }
        else if (newIndex >= buttons.Count) { newIndex = 0; }

        SelectButton(newIndex);
    }

    void SelectButton(int index)
    {
        if (index < 0 || index >= buttons.Count) return;

        // ���ѡ�����µİ�ť����ȡ����һ����ť��ѡ��״̬
        if (currentIndex != -1 && currentIndex != index && currentIndex < buttons.Count)
        {
            buttons[currentIndex].OnDeselected();
        }

        currentIndex = index;
        buttons[currentIndex].OnSelected();

        // ���� EventSystem ��ѡ�ж���������ֱ�֧�ֺ�ĳЩUI��������Ҫ
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
    }
}