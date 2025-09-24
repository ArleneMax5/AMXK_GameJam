// 枚举定义各类用于UI按钮与触发器的游戏事件
public enum GameEventType
{
    // --- 主菜单 ---
    StartNewGame,
    OpenLoadPanel,
    OpenSettings,
    QuitGame,

    // --- 暂停菜单 ---
    ResumeGame,
    OpenSavePanel,
    freshGuide,
    BackToMainMenu,

    // --- 存档/读档界面 ---
    CloseSaveLoadPanel,

    // --- 事件对话框 ---
    EventOption1,
    EventOption2,
    EventOption3,

    // --- 游戏控制 ---
    FastForward, // 快进一天

    // --- 游戏结束 ---
    RestartGame, // 重启游戏（从游戏结束界面）
}