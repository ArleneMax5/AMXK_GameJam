// 这个枚举定义了所有UI按钮可以触发的游戏事件
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

    // --- 存档/读档面板 ---
    CloseSaveLoadPanel,

    // --- 事件对话框 ---
    EventOption1,
    EventOption2,
    EventOption3,
}