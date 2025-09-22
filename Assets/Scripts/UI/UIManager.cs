using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("面板引用")]
    [SerializeField] private MainMenuPanel mainMenuPanel;
    [SerializeField] private GameUIPanel gameUIPanel;
    [SerializeField] private PauseMenuPanel pauseMenuPanel;
    [SerializeField] private EventDialogPanel eventDialogPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 游戏开始时，确保所有面板都处于正确的初始状态
        mainMenuPanel.gameObject.SetActive(false);
        gameUIPanel.gameObject.SetActive(false);
        pauseMenuPanel.gameObject.SetActive(false);
        eventDialogPanel.gameObject.SetActive(false);

        // 首先显示主菜单
        ShowMainMenu();
    }

    private void Update()
    {
        // 监听全局的暂停输入
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 如果当前在游戏内，则打开暂停菜单
            if (gameUIPanel.gameObject.activeSelf && !pauseMenuPanel.gameObject.activeSelf)
            {
                ShowPauseMenu();
            }
            // 如果暂停菜单已打开，则关闭它
            else if (pauseMenuPanel.gameObject.activeSelf)
            {
                HidePauseMenu();
            }
        }
    }

    // --- 公共的面板控制方法 ---

    public void ShowMainMenu()
    {
        mainMenuPanel.gameObject.SetActive(true);
        mainMenuPanel.OnPanelActive();
    }

    public void StartGame() // 这个方法可以由主菜单的“开始游戏”按钮调用
    {
        mainMenuPanel.OnPanelInactive();
        mainMenuPanel.gameObject.SetActive(false);

        gameUIPanel.gameObject.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        pauseMenuPanel.gameObject.SetActive(true);
        pauseMenuPanel.OnPanelActive();
        // Time.timeScale = 0f; // 暂停游戏
    }

    public void HidePauseMenu()
    {
        pauseMenuPanel.OnPanelInactive();
        pauseMenuPanel.gameObject.SetActive(false);
        // Time.timeScale = 1f; // 恢复游戏
    }

    public void ShowEventDialog(/* 事件数据 */)
    {
        eventDialogPanel.gameObject.SetActive(true);
        eventDialogPanel.ShowEvent(/* 事件数据 */);
    }
}