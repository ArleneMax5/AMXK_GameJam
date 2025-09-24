using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 核心方法：UI按钮通过这个触发一个事件
    public void TriggerEvent(GameEventType eventType)
    {
        Debug.Log("触发事件: " + eventType);

        // 根据不同的事件类型分发给确实的功能处理
        switch (eventType)
        {
            // --- 主菜单事件 ---
            case GameEventType.StartNewGame:
                // --- 重要修改 ---
                // 1. 让 GameManager 初始化游戏数据
                GameManager.Instance.InitializeNewGame();
                // 2. 让 UIManager 切换到游戏界面
                UIManager.Instance.ClearAndPushPanel(PanelType.GameUI);
                break;
            case GameEventType.OpenLoadPanel:
                // UIManager.Instance.ShowLoadPanel(); 
                // 暂未实现，预留
                break;
            case GameEventType.QuitGame:
                Application.Quit();
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                break;

            // --- 暂停菜单事件 ---
            case GameEventType.ResumeGame:
                // 只需要关闭暂停菜单就行
                UIManager.Instance.PopPanel();
                break;
            case GameEventType.OpenSavePanel:
                // UIManager.Instance.ShowSavePanel(); // 暂未实现，预留
                break;
            case GameEventType.BackToMainMenu:
                // 清空堆栈，返回主菜单
                Time.timeScale = 1f; // 重置时间缩放
                UIManager.Instance.ClearAndPushPanel(PanelType.MainMenu);
                break;

            // --- 游戏控制事件 ---
            case GameEventType.FastForward:
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.FastForwardDay();
                }
                break;

            // --- 游戏结束事件 ---
            case GameEventType.RestartGame:
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.RestartGame();
                }
                break;

            // ... 其他事件
        }
    }
}