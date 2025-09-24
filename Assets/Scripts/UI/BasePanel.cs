using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
// �ܽ᣺��� BasePanel ��ʵ����ͳһ�ļ��̵����������ͣѡ��͵��ȷ�ϰ�ť���߼���

// public enum PanelType
// {
//     MainMenu,
//     GameUI,
//     PauseMenu,
//     EventDialog,
//     Tutorial,
//     // ����ָ��
//     Interaction,
//     // �¼�����
//     EventResult,
//     // ����ʧ�����
//     GameOver
// }


[RequireComponent(typeof(Canvas))] // ǿ��Ҫ��ÿ����嶼��һ��Canvas���
public class BasePanel : MonoBehaviour
{
    // ��ÿ�����֪���Լ�������
    [SerializeField] private PanelType panelType;
    public PanelType PanelType => panelType;

    [Header("����ڵİ�ť�б�")]
    [SerializeField] protected List<SelectableButton> buttons;

    protected int currentIndex = -1;
    private bool isNavigationActive = false;
    private EventSystem eventSystem;
    private Canvas panelCanvas; // ��������Canvas���������

    protected virtual void Awake()
    {
        eventSystem = EventSystem.current;
        panelCanvas = GetComponent<Canvas>(); // ��ȡCanvas���
        if (panelCanvas == null)
        {
            Debug.LogError($"��� {gameObject.name} ȱ�� Canvas �����", this);
        }
    }

    // ��Ϊ��屻����ʱ����UIManager����
    public virtual void Show()
    {
        gameObject.SetActive(true);
        isNavigationActive = true;
        if (buttons != null && buttons.Count > 0)
        {
            SelectButton(0); // Ĭ��ѡ�е�һ��
        }
    }

    // ��Ϊ��屻����ʱ����UIManager����
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

    // �޸ģ��Ƴ��˶� overrideSorting �����ã���Ϊ��ֻ����Ƕ��Canvas
    public void SetSortOrder(int order)
    {
        if (panelCanvas != null)
        {
            // ���ڸ�Canvas������ֻ��Ҫֱ������ sortingOrder
            panelCanvas.sortingOrder = order;
        }
    }

    // ֻ������̵���
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

        // --- ����ȷ�ϰ�ť (�س�/�ո�) ---
        if (currentIndex != -1 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space)))
        {
            buttons[currentIndex].ActivateButton();
        }
    }

    void ChangeSelection(int direction)
    {
        if (buttons == null || buttons.Count == 0) return;

        // ���֮ǰû��ѡ���κΰ�ť����ӵ�һ����ʼ
        int newIndex = (currentIndex == -1) ? 0 : currentIndex + direction;

        // ʵ��ѭ��ѡ��
        if (newIndex < 0) { newIndex = buttons.Count - 1; }
        else if (newIndex >= buttons.Count) { newIndex = 0; }

        SelectButton(newIndex);
    }

    // �����˷������Ա� SelectableButton ���Ե�����
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

        // ���ѡ�����µİ�ť����ȡ����һ����ť��ѡ��״̬
        if (currentIndex != -1 && currentIndex != index && currentIndex < buttons.Count)
        {
            buttons[currentIndex].OnDeselected();
        }

        currentIndex = index;
        buttons[currentIndex].OnSelected();

        // ���� EventSystem ��ѡ�ж����Ա�ֱ��֧����Ҫ����ĳЩUI����
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
    }
}