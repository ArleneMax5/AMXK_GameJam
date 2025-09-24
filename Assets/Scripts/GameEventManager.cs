using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ���ķ�����UI��ťͨ���������һ���¼�
    public void TriggerEvent(GameEventType eventType)
    {
        Debug.Log("�����¼�: " + eventType);

        // ���ݲ�ͬ���¼����ͷַ���ȷʵ�Ĺ��ܴ���
        switch (eventType)
        {
            // --- ���˵��¼� ---
            case GameEventType.StartNewGame:
                // --- ��Ҫ�޸� ---
                // 1. �� GameManager ��ʼ����Ϸ����
                GameManager.Instance.InitializeNewGame();
                // 2. �� UIManager �л�����Ϸ����
                UIManager.Instance.ClearAndPushPanel(PanelType.GameUI);
                break;
            case GameEventType.OpenLoadPanel:
                // UIManager.Instance.ShowLoadPanel(); 
                // ��δʵ�֣�Ԥ��
                break;
            case GameEventType.QuitGame:
                Application.Quit();
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                break;

            // --- ��ͣ�˵��¼� ---
            case GameEventType.ResumeGame:
                // ֻ��Ҫ�ر���ͣ�˵�����
                UIManager.Instance.PopPanel();
                break;
            case GameEventType.OpenSavePanel:
                // UIManager.Instance.ShowSavePanel(); // ��δʵ�֣�Ԥ��
                break;
            case GameEventType.BackToMainMenu:
                // ��ն�ջ���������˵�
                Time.timeScale = 1f; // ����ʱ������
                UIManager.Instance.ClearAndPushPanel(PanelType.MainMenu);
                break;

            // --- ��Ϸ�����¼� ---
            case GameEventType.FastForward:
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.FastForwardDay();
                }
                break;

            // --- ��Ϸ�����¼� ---
            case GameEventType.RestartGame:
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.RestartGame();
                }
                break;

            // ... �����¼�
        }
    }
}