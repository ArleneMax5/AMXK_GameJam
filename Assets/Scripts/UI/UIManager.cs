using System.Collections.Generic;
using System.Linq; // 需要引入Linq
using UnityEngine;

// 新增一个枚举来标识所有面板类型
public enum PanelType
{
    MainMenu,
    GameUI,
    PauseMenu,
    EventDialog,
    Tutorial, // 新手指南
    Interaction, // 区块交互
    EventResult // 事件结果
}

// UIManager 负责管理所有UI面板的显示和隐藏，使用UI堆栈
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("面板设置")]
    [SerializeField] private Transform panelParent;
    [SerializeField] private List<BasePanel> panelPrefabs;
    
    private Dictionary<PanelType, BasePanel> _panelInstances = new Dictionary<PanelType, BasePanel>();
    private Stack<BasePanel> _panelStack = new Stack<BasePanel>();

    // 新增：定义基础排序顺序和每个面板之间的间隔
    private const int BASE_SORT_ORDER = 10;
    private const int SORT_ORDER_INCREMENT = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (panelParent == null)
            {
                Debug.LogError("UIManager: Panel Parent 未设置！", this);
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
            // 如果堆栈中有多个面板，或者只有一个非GameUI的面板，则弹出
            if (_panelStack.Count > 1 || (_panelStack.Count == 1 && _panelStack.Peek().PanelType != PanelType.GameUI))
            {
                PopPanel();
            }
            // 如果只剩下GameUI，就打开暂停菜单
            else if (_panelStack.Count == 1 && _panelStack.Peek().PanelType == PanelType.GameUI)
            {
                PushPanel(PanelType.PauseMenu);
            }
        }
    }

    // 修改：PushPanel不再隐藏之前的面板
    public void PushPanel(PanelType panelType)
    {
        if (_panelInstances.TryGetValue(panelType, out BasePanel panelToPush))
        {
            panelToPush.Show();
            _panelStack.Push(panelToPush);
            UpdatePanelsSortingOrder();
        }
    }

    // 修改：PopPanel不再显示新的顶部面板，只负责弹出和更新排序
    public void PopPanel()
    {
        if (_panelStack.Count <= 0) return;

        BasePanel panelToPop = _panelStack.Pop();
        panelToPop.Hide();

        UpdatePanelsSortingOrder();
    }

    // 新增：一个辅助方法，根据面板在堆栈中的顺序更新其Canvas排序
    private void UpdatePanelsSortingOrder()
    {
        int currentOrder = BASE_SORT_ORDER;
        // 我们需要从栈底到栈顶遍历，所以先将栈反转
        foreach (var panel in _panelStack.Reverse())
        {
            panel.SetSortOrder(currentOrder);
            currentOrder += SORT_ORDER_INCREMENT;
        }
    }

    // 关闭所有面板，然后打开一个全新的面板（用于主菜单->游戏界面的切换）
    public void ClearAndPushPanel(PanelType panelType)
    {
        while (_panelStack.Count > 0)
        {
            _panelStack.Pop().Hide();
        }
        PushPanel(panelType);
    }
}