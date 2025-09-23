using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuPanel : BasePanel
{
    [Header("���ã������գ�����ʱ�Զ����ң�")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private RegionMapUIController _mapController;

    protected override void Awake()
    {
        base.Awake();
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_mapController == null) _mapController = FindObjectOfType<RegionMapUIController>(true);
    }

    // ��/�ر���ͣʱ��ѡ�ؿ��� TimeScale��������Ŀ�Ƿ��õ���
    public override void Show()
    {
        base.Show();
        Time.timeScale = 0f;
    }
    public override void Hide()
    {
        Time.timeScale = 1f;
        base.Hide();
    }

    // ���� ���ĸ�����ֱ�Ӱ󵽰�ť OnClick() ���� 

    public void OnClickResume()
    {
        UIManager.Instance?.PopPanel(); // �ر���ͣ��壨�ص� GameUI��
    }

    public void OnClickSave()
    {
        SaveLoadManager.SaveGame(_gameManager, _mapController);
    }

    public void OnClickLoad()
    {
        bool ok = SaveLoadManager.LoadGame(_gameManager, _mapController);
        Debug.Log(ok ? "[Pause] �����ɹ�" : "[Pause] ����ʧ��/�޴浵");
    }

    public void OnClickQuitToMenu()
    {
        // ��������˵�����Ϸ��ͬһ������Ҳ������ UIManager �����˵���壺
        // UIManager.Instance?.ClearAndPushPanel(PanelType.MainMenu);
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene"); // �ĳ�������˵�������
    }
}
