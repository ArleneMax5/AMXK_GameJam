using UnityEngine;

public class GameOverPanel : BasePanel
{
    [Header("游戏结束界面")]
    [SerializeField] private GameObject backgroundOverlay; // 半透明背景遮罩

    public override void Show()
    {
        base.Show();
        
        // 确保覆盖层在最顶层
        if (backgroundOverlay != null)
        {
            backgroundOverlay.SetActive(true);
        }
        
        Debug.Log("显示游戏结束界面覆盖层");
    }

    public override void Hide()
    {
        if (backgroundOverlay != null)
        {
            backgroundOverlay.SetActive(false);
        }
        
        base.Hide();
        Debug.Log("隐藏游戏结束界面覆盖层");
    }
}