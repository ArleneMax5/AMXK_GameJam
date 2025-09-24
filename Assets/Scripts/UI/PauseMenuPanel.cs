using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuPanel : BasePanel
{
    [Header("引用（可留空，运行时自动查找）")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private RegionMapUIController _mapController;

    protected override void Awake()
    {
        base.Awake();
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_mapController == null) _mapController = FindObjectOfType<RegionMapUIController>(true);
    }

    // 打开/关闭暂停时可选地控制 TimeScale（看你项目是否用到）
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

    // —— 这四个方法直接绑到按钮 OnClick() —— 

    public void OnClickResume()
    {
        UIManager.Instance?.PopPanel(); // 关闭暂停面板（回到 GameUI）
    }

    public void OnClickSave()
    {
        SaveLoadManager.SaveGame(_gameManager, _mapController);
    }

    public void OnClickLoad()
    {
        bool ok = SaveLoadManager.LoadGame(_gameManager, _mapController);
        Debug.Log(ok ? "[Pause] 读档成功" : "[Pause] 读档失败/无存档");
    }

    public void OnClickQuitToMenu()
    {
        // 若你的主菜单与游戏在同一场景，也可以用 UIManager 回主菜单面板：
        // UIManager.Instance?.ClearAndPushPanel(PanelType.MainMenu);
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene"); // 改成你的主菜单场景名
    }
}
