using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("�������")]
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
        // ��Ϸ��ʼʱ��ȷ��������嶼������ȷ�ĳ�ʼ״̬
        mainMenuPanel.gameObject.SetActive(false);
        gameUIPanel.gameObject.SetActive(false);
        pauseMenuPanel.gameObject.SetActive(false);
        eventDialogPanel.gameObject.SetActive(false);

        // ������ʾ���˵�
        ShowMainMenu();
    }

    private void Update()
    {
        // ����ȫ�ֵ���ͣ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �����ǰ����Ϸ�ڣ������ͣ�˵�
            if (gameUIPanel.gameObject.activeSelf && !pauseMenuPanel.gameObject.activeSelf)
            {
                ShowPauseMenu();
            }
            // �����ͣ�˵��Ѵ򿪣���ر���
            else if (pauseMenuPanel.gameObject.activeSelf)
            {
                HidePauseMenu();
            }
        }
    }

    // --- �����������Ʒ��� ---

    public void ShowMainMenu()
    {
        mainMenuPanel.gameObject.SetActive(true);
        mainMenuPanel.OnPanelActive();
    }

    public void StartGame() // ����������������˵��ġ���ʼ��Ϸ����ť����
    {
        mainMenuPanel.OnPanelInactive();
        mainMenuPanel.gameObject.SetActive(false);

        gameUIPanel.gameObject.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        pauseMenuPanel.gameObject.SetActive(true);
        pauseMenuPanel.OnPanelActive();
        // Time.timeScale = 0f; // ��ͣ��Ϸ
    }

    public void HidePauseMenu()
    {
        pauseMenuPanel.OnPanelInactive();
        pauseMenuPanel.gameObject.SetActive(false);
        // Time.timeScale = 1f; // �ָ���Ϸ
    }

    public void ShowEventDialog(/* �¼����� */)
    {
        eventDialogPanel.gameObject.SetActive(true);
        eventDialogPanel.ShowEvent(/* �¼����� */);
    }
}