// ö�ٶ����������UI��ť�봥��������Ϸ�¼�
public enum GameEventType
{
    // --- ���˵� ---
    StartNewGame,
    OpenLoadPanel,
    OpenSettings,
    QuitGame,

    // --- ��ͣ�˵� ---
    ResumeGame,
    OpenSavePanel,
    freshGuide,
    BackToMainMenu,

    // --- �浵/�������� ---
    CloseSaveLoadPanel,

    // --- �¼��Ի��� ---
    EventOption1,
    EventOption2,
    EventOption3,

    // --- ��Ϸ���� ---
    FastForward, // ���һ��

    // --- ��Ϸ���� ---
    RestartGame, // ������Ϸ������Ϸ�������棩
}