using System.Collections.Generic;
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

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // --- 核心修改 1: 添加一个 Transform 来指定面板的父对象 ---
    [Header("面板设置")]
    [SerializeField] private Transform panelParent; // 将你的 Canvas 对象拖到这里
    [SerializeField] private List<BasePanel> panelPrefabs;
    
    private Dictionary<PanelType, BasePanel> _panelInstances = new Dictionary<PanelType, BasePanel>();

    // 核心：UI堆栈
    private Stack<BasePanel> _panelStack = new Stack<BasePanel>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // --- 核心修改 2: 检查 panelParent 是否已设置 ---
            if (panelParent == null)
            {
                Debug.LogError("UIManager: Panel Parent 未设置！请将 Canvas 或一个用于容纳面板的子对象拖拽到 UIManager 的 Panel Parent 字段上。", this);
                return;
            }

            // 在游戏开始时实例化所有面板并禁用，放入字典
            foreach (var panelPrefab in panelPrefabs)
            {
                // --- 核心修改 3: 使用 panelParent 作为实例化的父对象 ---
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
        // 游戏开始时，只显示主菜单
        PushPanel(PanelType.MainMenu);
    }

    private void Update()
    {
        // 全局的返回/暂停逻辑，现在变得极其简单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 如果堆栈中有面板，就弹出一个
            if (_panelStack.Count > 1)
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

    /// <summary>
    /// 推入一个新面板到栈顶（显示新面板，可选择性隐藏旧的）
    /// </summary>
    public void PushPanel(PanelType panelType)
    {
        // 隐藏当前的栈顶面板
        if (_panelStack.Count > 0)
        {
            _panelStack.Peek().Hide();
        }

        BasePanel panelToPush = _panelInstances[panelType];
        panelToPush.Show();
        _panelStack.Push(panelToPush);
    }

    /// <summary>
    /// 从栈顶弹出一个面板（关闭当前面板，并显示上一个）
    /// </summary>
    public void PopPanel()
    {
        if (_panelStack.Count <= 0) return;

        // 弹出并隐藏当前栈顶
        BasePanel panelToPop = _panelStack.Pop();
        panelToPop.Hide();

        // 重新显示新的栈顶面板
        if (_panelStack.Count > 0)
        {
            _panelStack.Peek().Show();
        }
    }

    /// <summary>
    /// 关闭所有面板，然后打开一个全新的面板（用于主菜单->游戏界面的切换）
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