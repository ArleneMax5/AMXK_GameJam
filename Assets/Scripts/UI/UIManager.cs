using System.Collections.Generic;
using UnityEngine;

// ����һ��ö������ʶ�����������
public enum PanelType
{
    MainMenu,
    GameUI,
    PauseMenu,
    EventDialog,
    Tutorial, // ����ָ��
    Interaction, // ���齻��
    EventResult // �¼����
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // --- �����޸� 1: ���һ�� Transform ��ָ�����ĸ����� ---
    [Header("�������")]
    [SerializeField] private Transform panelParent; // ����� Canvas �����ϵ�����
    [SerializeField] private List<BasePanel> panelPrefabs;
    
    private Dictionary<PanelType, BasePanel> _panelInstances = new Dictionary<PanelType, BasePanel>();

    // ���ģ�UI��ջ
    private Stack<BasePanel> _panelStack = new Stack<BasePanel>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // --- �����޸� 2: ��� panelParent �Ƿ������� ---
            if (panelParent == null)
            {
                Debug.LogError("UIManager: Panel Parent δ���ã��뽫 Canvas ��һ���������������Ӷ�����ק�� UIManager �� Panel Parent �ֶ��ϡ�", this);
                return;
            }

            // ����Ϸ��ʼʱʵ����������岢���ã������ֵ�
            foreach (var panelPrefab in panelPrefabs)
            {
                // --- �����޸� 3: ʹ�� panelParent ��Ϊʵ�����ĸ����� ---
                var panelInstance = Instantiate(panelPrefab, panelParent);
                panelInstance.gameObject.SetActive(false);
                _panelInstances.Add(panelInstance.PanelType, panelInstance);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ��Ϸ��ʼʱ��ֻ��ʾ���˵�
        PushPanel(PanelType.MainMenu);
    }

    private void Update()
    {
        // ȫ�ֵķ���/��ͣ�߼������ڱ�ü����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �����ջ������壬�͵���һ��
            if (_panelStack.Count > 1)
            {
                PopPanel();
            }
            // ���ֻʣ��GameUI���ʹ���ͣ�˵�
            else if (_panelStack.Count == 1 && _panelStack.Peek().PanelType == PanelType.GameUI)
            {
                PushPanel(PanelType.PauseMenu);
            }
        }
    }

    /// <summary>
    /// ����һ������嵽ջ������ʾ����壬��ѡ�������ؾɵģ�
    /// </summary>
    public void PushPanel(PanelType panelType)
    {
        // ���ص�ǰ��ջ�����
        if (_panelStack.Count > 0)
        {
            _panelStack.Peek().Hide();
        }

        BasePanel panelToPush = _panelInstances[panelType];
        panelToPush.Show();
        _panelStack.Push(panelToPush);
    }

    /// <summary>
    /// ��ջ������һ����壨�رյ�ǰ��壬����ʾ��һ����
    /// </summary>
    public void PopPanel()
    {
        if (_panelStack.Count <= 0) return;

        // ���������ص�ǰջ��
        BasePanel panelToPop = _panelStack.Pop();
        panelToPop.Hide();

        // ������ʾ�µ�ջ�����
        if (_panelStack.Count > 0)
        {
            _panelStack.Peek().Show();
        }
    }

    /// <summary>
    /// �ر�������壬Ȼ���һ��ȫ�µ���壨�������˵�->��Ϸ������л���
    /// </summary>
    public void ClearAndPushPanel(PanelType panelType)
    {
        while (_panelStack.Count > 0)
        {
            _panelStack.Pop().Hide();
        }
        PushPanel(panelType);
    }
}