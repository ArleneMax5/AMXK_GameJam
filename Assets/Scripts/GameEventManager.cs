using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ���Ǻ��ķ�����UI��ť��������������һ���¼�
    public void TriggerEvent(GameEventType eventType)
    {
        Debug.Log("�����¼�: " + eventType);

        // ��������ǽ��¼��ַ�����ȷ�Ĺ�����
        switch (eventType)
        {
            // --- ���˵��¼� ---
            case GameEventType.StartNewGame:
                // --- �����޸� ---
                // 1. �� GameManager ��ʼ����Ϸ����
                GameManager.Instance.InitializeNewGame();
                // 2. �� UIManager �л�����Ϸ����
                UIManager.Instance.ClearAndPushPanel(PanelType.GameUI);
                break;
            case GameEventType.OpenLoadPanel:
                // UIManager.Instance.ShowLoadPanel(); 
                // �������������
                break;
            case GameEventType.QuitGame:
                Application.Quit();
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                break;

            // --- ��ͣ�˵��¼� ---
            case GameEventType.ResumeGame:
                // ֻ��Ҫ������ͣ�˵�����
                UIManager.Instance.PopPanel();
                break;
            case GameEventType.OpenSavePanel:
                // UIManager.Instance.ShowSavePanel(); // �������������
                break;
            case GameEventType.BackToMainMenu:
                // ��ն�ջ���ص����˵�
                UIManager.Instance.ClearAndPushPanel(PanelType.MainMenu);
                break;

            // ... �����¼�
        }
    }
}