using System.Linq; // ��Ҫ����Linq
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
    EventResult, // �¼����
    GameOver // ʧ�����
}

// UIManager �����������UI������ʾ�����أ�ʹ��UI��ջ
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("�������")]
    [SerializeField] private Transform panelParent;
    [SerializeField] private List<BasePanel> panelPrefabs;
    
    private Dictionary<PanelType, BasePanel> _panelInstances = new Dictionary<PanelType, BasePanel>();
    private Stack<BasePanel> _panelStack = new Stack<BasePanel>();

    // �����������������˳���ÿ�����֮��ļ��
    private const int BASE_SORT_ORDER = 10;
    private const int SORT_ORDER_INCREMENT = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (panelParent == null)
            {
                Debug.LogError("UIManager: Panel Parent δ���ã�", this);
                return;
            }

            foreach (var panelPrefab in panelPrefabs)
            {
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
        PushPanel(PanelType.MainMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �����ջ���ж����壬����ֻ��һ����GameUI����壬�򵯳�
            if (_panelStack.Count > 1 || (_panelStack.Count == 1 && _panelStack.Peek().PanelType != PanelType.GameUI))
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

    // �޸ģ�PushPanel��������֮ǰ�����
    public void PushPanel(PanelType panelType)
    {
        if (_panelInstances.TryGetValue(panelType, out BasePanel panelToPush))
        {
            panelToPush.Show();
            _panelStack.Push(panelToPush);
            UpdatePanelsSortingOrder();
        }
    }

    // �޸ģ�PopPanel������ʾ�µĶ�����壬ֻ���𵯳��͸�������
    public void PopPanel()
    {
        if (_panelStack.Count <= 0) return;

        BasePanel panelToPop = _panelStack.Pop();
        panelToPop.Hide();

        UpdatePanelsSortingOrder();
    }

    // ������һ��������������������ڶ�ջ�е�˳�������Canvas����
    private void UpdatePanelsSortingOrder()
    {
        int currentOrder = BASE_SORT_ORDER;
        // ������Ҫ��ջ�׵�ջ�������������Ƚ�ջ��ת
        foreach (var panel in _panelStack.Reverse())
        {
            panel.SetSortOrder(currentOrder);
            currentOrder += SORT_ORDER_INCREMENT;
        }
    }

    // �ر�������壬Ȼ���һ��ȫ�µ���壨�������˵�->��Ϸ������л���
    public void ClearAndPushPanel(PanelType panelType)
    {
        while (_panelStack.Count > 0)
        {
            _panelStack.Pop().Hide();
        }
        PushPanel(panelType);
    }
}