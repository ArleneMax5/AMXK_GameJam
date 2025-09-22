using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 这是核心方法，UI按钮将调用它来触发一个事件
    public void TriggerEvent(GameEventType eventType)
    {
        Debug.Log("触发事件: " + eventType);

        // 在这里，我们将事件分发给正确的管理器
        switch (eventType)
        {
            // --- 主菜单事件 ---
            case GameEventType.StartNewGame:
                // --- 核心修改 ---
                // 1. 让 GameManager 初始化游戏数据
                GameManager.Instance.InitializeNewGame();
                // 2. 让 UIManager 切换到游戏界面
                UIManager.Instance.ClearAndPushPanel(PanelType.GameUI);
                break;
            case GameEventType.OpenLoadPanel:
                // UIManager.Instance.ShowLoadPanel(); 
                // 假设有这个方法
                break;
            case GameEventType.QuitGame:
                Application.Quit();
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                break;

            // --- 暂停菜单事件 ---
            case GameEventType.ResumeGame:
                // 只需要弹出暂停菜单即可
                UIManager.Instance.PopPanel();
                break;
            case GameEventType.OpenSavePanel:
                // UIManager.Instance.ShowSavePanel(); // 假设有这个方法
                break;
            case GameEventType.BackToMainMenu:
                // 清空堆栈并回到主菜单
                UIManager.Instance.ClearAndPushPanel(PanelType.MainMenu);
                break;

            // ... 其他事件
        }
    }
}